using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StructuredLogViewer.LLM.GitHubCopilot.Authentication;
using StructuredLogViewer.LLM.GitHubCopilot.Models;

namespace StructuredLogViewer.LLM.GitHubCopilot.Client
{
    /// <summary>
    /// Client for GitHub Copilot chat completions API.
    /// </summary>
    public class GitHubCopilotChatCompletionsClient : IDisposable
    {
        private readonly GitHubCopilotHttpClient httpClient;
        private readonly string defaultModel;

        public GitHubCopilotChatCompletionsClient(
            GitHubCopilotHttpClient httpClient,
            string defaultModel = "gpt-4")
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.defaultModel = defaultModel ?? "gpt-4";
        }

        /// <summary>
        /// Creates a chat completion.
        /// </summary>
        public async Task<ChatCompletionResponse> CreateCompletionAsync(
            ChatCompletionRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Ensure model is set
            request.Model ??= defaultModel;

            // Ensure stream is false for non-streaming
            request.Stream = false;

            // Determine if this is agent-initiated (has assistant or tool messages)
            var isAgentInitiated = request.Messages.Any(m =>
                m.Role == "assistant" || m.Role == "tool");

            var httpRequest = httpClient.CreateRequest(HttpMethod.Post, "chat/completions", isAgentInitiated);
            httpRequest.Headers.Add("X-Interaction-Type", "chat");

            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
            // Remove charset from Content-Type (Copilot API requirement)
            httpRequest.Content.Headers.ContentType.CharSet = null;

            var response = await httpClient.SendAsync(httpRequest, cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Copilot API returned {response.StatusCode}: {responseContent}");
            }

            return JsonSerializer.Deserialize<ChatCompletionResponse>(responseContent);
        }

        /// <summary>
        /// Creates a streaming chat completion.
        /// </summary>
        public async IAsyncEnumerable<ChatCompletionChunk> CreateStreamingCompletionAsync(
            ChatCompletionRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Ensure model is set
            request.Model ??= defaultModel;

            // Enable streaming
            request.Stream = true;

            // Determine if this is agent-initiated
            var isAgentInitiated = request.Messages.Any(m =>
                m.Role == "assistant" || m.Role == "tool");

            var httpRequest = httpClient.CreateRequest(HttpMethod.Post, "v1/chat/completions", isAgentInitiated);
            httpRequest.Headers.Add("X-Interaction-Type", "chat");

            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
            httpRequest.Content.Headers.ContentType.CharSet = null;

            var response = await httpClient.SendAsync(httpRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Copilot API returned {response.StatusCode}: {errorContent}");
            }

            // Read streaming response
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            string line;
            while ((line = await reader.ReadLineAsync()) != null && !cancellationToken.IsCancellationRequested)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // SSE format: "data: {json}" or "data: [DONE]"
                if (line.StartsWith("data: "))
                {
                    var data = line.Substring(6).Trim();

                    if (data == "[DONE]")
                    {
                        yield break;
                    }

                    ChatCompletionChunk chunk;
                    try
                    {
                        chunk = JsonSerializer.Deserialize<ChatCompletionChunk>(data);
                    }
                    catch (JsonException)
                    {
                        // Skip malformed chunks
                        continue;
                    }

                    if (chunk != null)
                    {
                        yield return chunk;
                    }
                }
            }
        }

        public void Dispose()
        {
            // httpClient is owned by caller, don't dispose
        }
    }
}
