using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using StructuredLogViewer.LLM.GitHubCopilot.Authentication;
using StructuredLogViewer.LLM.GitHubCopilot.Models;
using AIMessage = Microsoft.Extensions.AI.ChatMessage;

namespace StructuredLogViewer.LLM.GitHubCopilot.Client
{
    /// <summary>
    /// IChatClient implementation for GitHub Copilot.
    /// </summary>
    public class GitHubCopilotChatClient : IChatClient
    {
        private readonly GitHubCopilotTokenProvider tokenProvider;
        private readonly GitHubCopilotHttpClient httpClient;
        private readonly GitHubCopilotChatCompletionsClient completionsClient;
        private readonly string modelName;

        public ChatClientMetadata Metadata { get; }

        public GitHubCopilotChatClient(
            GitHubCopilotTokenProvider tokenProvider,
            string modelName = "gpt-4",
            string baseUrl = null)
        {
            this.tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            this.modelName = modelName ?? "gpt-4";
            this.httpClient = new GitHubCopilotHttpClient(tokenProvider, baseUrl);
            this.completionsClient = new GitHubCopilotChatCompletionsClient(httpClient, modelName);

            Metadata = new ChatClientMetadata("GitHub Copilot", new Uri("https://github.com/features/copilot"), modelName);
        }

        public async Task<ChatResponse> GetResponseAsync(
            IEnumerable<AIMessage> messages,
            ChatOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var request = MapToCopilotRequest(messages, options);
            var response = await completionsClient.CreateCompletionAsync(request, cancellationToken);
            return MapToChatResponse(response);
        }

        object IChatClient.GetService(Type serviceType, object serviceKey)
        {
            return null;
        }

        void IDisposable.Dispose()
        {
            httpClient?.Dispose();
            completionsClient?.Dispose();
        }

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<AIMessage> messages,
            ChatOptions options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var request = MapToCopilotRequest(messages, options);

            await foreach (var chunk in completionsClient.CreateStreamingCompletionAsync(request, cancellationToken))
            {
                if (chunk.Choices == null || chunk.Choices.Count == 0)
                    continue;

                var choice = chunk.Choices[0];
                
                // Create contents list
                var contents = new List<AIContent>();
                
                // Add text content if present
                if (!string.IsNullOrEmpty(choice.Delta?.Content))
                {
                    contents.Add(new TextContent(choice.Delta.Content));
                }
                
                // Add tool calls if present
                if (choice.Delta?.ToolCalls != null && choice.Delta.ToolCalls.Count > 0)
                {
                    contents.AddRange(choice.Delta.ToolCalls.Select(tc => MapToolCallUpdate(tc)));
                }

                var update = new ChatResponseUpdate
                {
                    Role = choice.Delta?.Role != null ? MapRole(choice.Delta.Role) : null,
                    Contents = contents.Count > 0 ? contents : null,
                    FinishReason = MapFinishReason(choice.FinishReason)
                };

                yield return update;
            }
        }

        private ChatCompletionRequest MapToCopilotRequest(IEnumerable<AIMessage> messages, ChatOptions options)
        {
            var request = new ChatCompletionRequest
            {
                Model = options?.ModelId ?? modelName,
                Messages = messages.Select(MapMessage).ToList(),
                Temperature = options?.Temperature,
                TopP = options?.TopP,
                MaxTokens = options?.MaxOutputTokens,
                Stop = options?.StopSequences?.ToList(),
                FrequencyPenalty = options?.FrequencyPenalty,
                PresencePenalty = options?.PresencePenalty
            };

            // Map tools if provided
            if (options?.Tools != null && options.Tools.Count > 0)
            {
                request.Tools = options.Tools.Select(MapTool).ToList();

                // Set tool choice based on mode
                var toolMode = options.ToolMode;
                if (toolMode != null)
                {
                    request.ToolChoice = "required";
                }
                else
                {
                    request.ToolChoice = "auto";
                }
            }

            return request;
        }

        private Models.ChatMessage MapMessage(AIMessage message)
        {
            var copilotMessage = new Models.ChatMessage
            {
                Role = message.Role.Value.ToLowerInvariant(),
                Content = message.Text
            };

            // Handle tool calls from assistant
            if (message.Contents != null)
            {
                var toolCalls = message.Contents
                    .OfType<FunctionCallContent>()
                    .Select(fc => new ToolCall
                    {
                        Id = fc.CallId,
                        Type = "function",
                        Function = new FunctionCall
                        {
                            Name = fc.Name,
                            Arguments = JsonSerializer.Serialize(fc.Arguments)
                        }
                    })
                    .ToList();

                if (toolCalls.Count > 0)
                {
                    copilotMessage.ToolCalls = toolCalls;
                }

                // Handle tool results
                var toolResult = message.Contents.OfType<FunctionResultContent>().FirstOrDefault();
                if (toolResult != null)
                {
                    copilotMessage.Role = "tool";
                    copilotMessage.ToolCallId = toolResult.CallId;
                    copilotMessage.Content = toolResult.Result?.ToString() ?? string.Empty;
                }
            }

            return copilotMessage;
        }

        private Tool MapTool(AITool aiTool)
        {
            if (aiTool is AIFunction aiFunction)
            {
                return new Tool
                {
                    Type = "function",
                    Function = new FunctionDefinition
                    {
                        Name = aiFunction.Name,
                        Description = aiFunction.Description,
                        Parameters = aiFunction.JsonSchema
                    }
                };
            }

            throw new NotSupportedException($"Tool type {aiTool.GetType().Name} is not supported.");
        }

        private ChatResponse MapToChatResponse(ChatCompletionResponse response)
        {
            if (response.Choices == null || response.Choices.Count == 0)
            {
                throw new InvalidOperationException("Copilot API returned no choices.");
            }

            var choice = response.Choices[0];
            var message = choice.Message;

            AIMessage chatMessage;

            // Map tool calls
            if (message.ToolCalls != null && message.ToolCalls.Count > 0)
            {
                var toolCalls = message.ToolCalls.Select(tc => new FunctionCallContent(
                    callId: tc.Id,
                    name: tc.Function.Name,
                    arguments: ParseArguments(tc.Function.Arguments)
                )).ToList<AIContent>();

                chatMessage = new AIMessage(MapRole(message.Role), toolCalls);
            }
            else
            {
                chatMessage = new AIMessage(MapRole(message.Role), message.Content);
            }

            var chatResponse = new ChatResponse(chatMessage)
            {
                FinishReason = MapFinishReason(choice.FinishReason),
                ModelId = response.Model
            };

            // Add usage information
            if (response.Usage != null)
            {
                chatResponse.Usage = new UsageDetails
                {
                    InputTokenCount = response.Usage.PromptTokens,
                    OutputTokenCount = response.Usage.CompletionTokens,
                    TotalTokenCount = response.Usage.TotalTokens
                };
            }

            return chatResponse;
        }

        private ChatRole MapRole(string role)
        {
            return role?.ToLowerInvariant() switch
            {
                "system" => ChatRole.System,
                "user" => ChatRole.User,
                "assistant" => ChatRole.Assistant,
                "tool" => ChatRole.Tool,
                _ => ChatRole.User
            };
        }

        private ChatFinishReason? MapFinishReason(string finishReason)
        {
            return finishReason?.ToLowerInvariant() switch
            {
                "stop" => ChatFinishReason.Stop,
                "length" => ChatFinishReason.Length,
                "tool_calls" => ChatFinishReason.ToolCalls,
                "content_filter" => ChatFinishReason.ContentFilter,
                null => null,
                _ => null
            };
        }

        private FunctionCallContent MapToolCallUpdate(ToolCall toolCall)
        {
            return new FunctionCallContent(
                callId: toolCall.Id,
                name: toolCall.Function?.Name,
                arguments: ParseArguments(toolCall.Function?.Arguments)
            );
        }

        private IDictionary<string, object> ParseArguments(string argumentsJson)
        {
            if (string.IsNullOrWhiteSpace(argumentsJson))
                return new Dictionary<string, object>();

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object>>(argumentsJson);
            }
            catch
            {
                return new Dictionary<string, object> { ["raw"] = argumentsJson };
            }
        }

        public object GetService(Type serviceType, object serviceKey = null)
        {
            // Return the completions client if requested
            if (serviceType == typeof(GitHubCopilotChatCompletionsClient))
            {
                return completionsClient;
            }

            return null;
        }

        public void Dispose()
        {
            completionsClient?.Dispose();
            httpClient?.Dispose();
        }
    }
}
