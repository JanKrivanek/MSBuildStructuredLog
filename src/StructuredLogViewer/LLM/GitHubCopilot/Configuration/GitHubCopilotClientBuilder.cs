using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using StructuredLogViewer.LLM.GitHubCopilot.Authentication;
using StructuredLogViewer.LLM.GitHubCopilot.Client;

namespace StructuredLogViewer.LLM.GitHubCopilot.Configuration
{
    /// <summary>
    /// Builder for creating GitHub Copilot IChatClient instances.
    /// </summary>
    public class GitHubCopilotClientBuilder
    {
        private GitHubCopilotConfiguration config = new GitHubCopilotConfiguration();
        private Action<string, string> deviceCodeCallback;

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        public GitHubCopilotClientBuilder WithConfiguration(GitHubCopilotConfiguration configuration)
        {
            config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            return this;
        }

        /// <summary>
        /// Sets the device code callback for authentication.
        /// </summary>
        /// <param name="callback">Callback with (userCode, verificationUrl)</param>
        public GitHubCopilotClientBuilder WithDeviceCodeCallback(Action<string, string> callback)
        {
            deviceCodeCallback = callback;
            return this;
        }

        /// <summary>
        /// Builds the IChatClient.
        /// </summary>
        public async Task<IChatClient> BuildAsync(CancellationToken cancellationToken = default)
        {
            if (!config.IsValid())
            {
                throw new InvalidOperationException("Configuration is not valid. " + config.GetStatusMessage());
            }

            // Get GitHub token (either from config or device flow)
            var githubToken = await GetGitHubTokenAsync(cancellationToken);

            // Create token provider
            var tokenProvider = new GitHubCopilotTokenProvider(githubToken, config.AccountType);

            // Get Copilot token
            var copilotToken = await tokenProvider.GetCopilotTokenAsync(cancellationToken);

            // Start auto-refresh if enabled
            if (config.AutoRefreshToken)
            {
                tokenProvider.StartAutoRefresh(copilotToken);
            }

            // Create the chat client
            var chatClient = new GitHubCopilotChatClient(
                tokenProvider,
                config.DefaultModel,
                config.BaseUrl);

            // Wrap with function invocation support
            IChatClient finalClient = new ChatClientBuilder(chatClient)
                .UseFunctionInvocation()
                .Build();

            return finalClient;
        }

        private async Task<string> GetGitHubTokenAsync(CancellationToken cancellationToken)
        {
            // Check if token is provided in config
            if (!string.IsNullOrWhiteSpace(config.GitHubToken))
            {
                return config.GitHubToken;
            }

            // Need to do device flow
            if (deviceCodeCallback == null)
            {
                throw new InvalidOperationException(
                    "No GitHub token available and no device code callback configured. " +
                    "Either provide a token in configuration or set up device code callback using WithDeviceCodeCallback().");
            }

            var deviceFlowAuthenticator = new GitHubDeviceFlowAuthenticator(deviceCodeCallback);
            return await deviceFlowAuthenticator.AuthenticateAsync(cancellationToken);
        }
    }
}
