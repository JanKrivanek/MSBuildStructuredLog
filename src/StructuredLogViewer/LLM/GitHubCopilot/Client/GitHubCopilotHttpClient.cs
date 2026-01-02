using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StructuredLogViewer.LLM.GitHubCopilot.Authentication;
using StructuredLogViewer.LLM.GitHubCopilot.Models;

namespace StructuredLogViewer.LLM.GitHubCopilot.Client
{
    /// <summary>
    /// HTTP client for GitHub Copilot API with required headers and authentication.
    /// </summary>
    public class GitHubCopilotHttpClient : IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly GitHubCopilotTokenProvider tokenProvider;
        private readonly string baseUrl;

        // Standard headers required by Copilot API
        private const string UserAgent = "GitHubCopilotChat/0.35.0";
        private const string EditorVersion = "vscode/1.107.0";
        private const string EditorPluginVersion = "copilot-chat/0.35.0";
        private const string CopilotIntegrationId = "vscode-chat";
        private const string OpenAIIntent = "conversation-panel";
        private const string GitHubApiVersion = "2022-11-28";

        public GitHubCopilotHttpClient(GitHubCopilotTokenProvider tokenProvider, string baseUrl = null)
        {
            this.tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            this.baseUrl = baseUrl;
            this.httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
        }

        /// <summary>
        /// Creates an HTTP request with all required headers.
        /// </summary>
        public HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, bool isAgentInitiated = false)
        {
            var token = tokenProvider.GetCurrentToken();
            if (token == null || token.IsExpired)
            {
                throw new InvalidOperationException("Copilot token is not available or has expired.");
            }

            var url = GetBaseUrl(token);
            var request = new HttpRequestMessage(method, $"{url}/{endpoint.TrimStart('/')}");

            // Standard headers
            request.Headers.Add("User-Agent", UserAgent);
            request.Headers.Add("Editor-Version", EditorVersion);
            request.Headers.Add("Editor-Plugin-Version", EditorPluginVersion);
            request.Headers.Add("Copilot-Integration-Id", CopilotIntegrationId);
            request.Headers.Add("openai-intent", OpenAIIntent);
            request.Headers.Add("x-request-id", Guid.NewGuid().ToString());
            request.Headers.Add("Accept", "application/json");

            // Authentication - use TryAddWithoutValidation because the Copilot token contains special characters
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token.Token}");

            // Initiator header (user or agent)
            request.Headers.Add("X-Initiator", isAgentInitiated ? "agent" : "user");

            return request;
        }

        /// <summary>
        /// Sends an HTTP request.
        /// Retry logic is handled by ResilientChatClient wrapper.
        /// </summary>
        public Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            return httpClient.SendAsync(request, cancellationToken);
        }

        private string GetBaseUrl(CopilotToken token)
        {
            return baseUrl ?? token?.BaseUrl ?? "https://api.individual.githubcopilot.com";
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
