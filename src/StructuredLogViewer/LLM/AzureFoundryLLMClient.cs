using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anthropic;
using Azure;
using Azure.AI.Inference;
using Azure.AI.OpenAI;
using Azure.Core;
using Microsoft.Extensions.AI;
using StructuredLogViewer.LLM.GitHubCopilot.Configuration;

namespace StructuredLogViewer.LLM
{
    /// <summary>
    /// Wrapper for Azure AI clients (OpenAI, Inference, Anthropic, or GitHub Copilot) implementing IChatClient.
    /// </summary>
    public class AzureFoundryLLMClient : IDisposable
    {
        private IChatClient chatClient;
        private ResilientChatClient resilientClient;
        private readonly string modelName;
        private readonly LLMConfiguration config;
        private Action<string, string> deviceCodeCallback;
        private bool isInitialized;

        public AzureFoundryLLMClient(LLMConfiguration config, Action<string, string> githubDeviceCodeCallback = null)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!config.IsConfigured)
            {
                throw new InvalidOperationException("LLM configuration is incomplete.");
            }

            this.config = config;
            this.deviceCodeCallback = githubDeviceCodeCallback;
            this.modelName = config.ModelName;
            this.isInitialized = false;

            // For non-GitHub Copilot providers, initialize synchronously
            if (config.Type != LLMConfiguration.ClientType.GitHubCopilot)
            {
                InitializeSynchronousClient();
                isInitialized = true;
            }
        }

        private void InitializeSynchronousClient()
        {
            var endpoint = new Uri(config.Endpoint);
            var credential = new AzureKeyCredential(config.ApiKey);

            if (config.Type == LLMConfiguration.ClientType.Anthropic)
            {
                chatClient = new AnthropicClient(
                    new Anthropic.Core.ClientOptions()
                    {
                        BaseUrl = endpoint,
                        APIKey = config.ApiKey,
                    })
                    .AsIChatClient(modelName);
            }
            else if (config.Type == LLMConfiguration.ClientType.AzureOpenAI)
            {
                var openAIClient = new AzureOpenAIClient(endpoint, credential);
                chatClient = openAIClient.GetChatClient(modelName).AsIChatClient();
            }
            else
            {
                var inferenceClient = new ChatCompletionsClient(endpoint, credential);
                chatClient = inferenceClient.AsIChatClient(modelName);
            }

            // Wrap with resilient client for automatic retry on rate limits and transient errors
            resilientClient = new ResilientChatClient(chatClient, maxRetries: 10);
            chatClient = resilientClient;
            
            // Apply function invocation after resilient wrapper
            chatClient = new ChatClientBuilder(chatClient).UseFunctionInvocation().Build();
        }

        /// <summary>
        /// Asynchronously initializes the client. Required for GitHub Copilot.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (isInitialized)
            {
                return;
            }

            if (config.Type == LLMConfiguration.ClientType.GitHubCopilot)
            {
                chatClient = await CreateGitHubCopilotClientAsync(config);
                // GitHub Copilot client is already wrapped with ResilientChatClient in CreateGitHubCopilotClientAsync
                resilientClient = chatClient as ResilientChatClient;
            }

            isInitialized = true;
        }

        private async Task<IChatClient> CreateGitHubCopilotClientAsync(LLMConfiguration config)
        {
            var ghConfig = new GitHubCopilotConfiguration
            {
                GitHubToken = config.ApiKey,
                DefaultModel = config.ModelName,
                BaseUrl = config.Endpoint.Contains("githubcopilot.com", StringComparison.OrdinalIgnoreCase) 
                    ? config.Endpoint 
                    : null
            };

            var builder = new GitHubCopilotClientBuilder()
                .WithConfiguration(ghConfig);

            if (deviceCodeCallback != null)
            {
                builder = builder.WithDeviceCodeCallback(deviceCodeCallback);
            }

            var client = await builder.BuildAsync();
            
            // Wrap with resilient client for automatic retry on rate limits and transient errors
            var resilientClient = new ResilientChatClient(client, maxRetries: 10);
            return resilientClient;
        }

        public IChatClient ChatClient => chatClient;

        /// <summary>
        /// Access to the resilient client for event subscription
        /// </summary>
        public ResilientChatClient ResilientClient => resilientClient;

        public void Dispose()
        {
            chatClient.Dispose();
        }
    }
}
