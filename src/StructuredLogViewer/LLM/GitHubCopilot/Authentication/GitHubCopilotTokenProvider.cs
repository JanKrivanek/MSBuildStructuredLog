using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using StructuredLogViewer.LLM.GitHubCopilot.Models;

namespace StructuredLogViewer.LLM.GitHubCopilot.Authentication
{
    /// <summary>
    /// Account type for GitHub Copilot.
    /// </summary>
    public enum CopilotAccountType
    {
        Individual,
        Business,
        Enterprise
    }

    /// <summary>
    /// Manages GitHub Copilot tokens - exchanges GitHub access tokens for Copilot tokens
    /// and handles token refresh.
    /// </summary>
    public class GitHubCopilotTokenProvider : IDisposable
    {
        private const string CopilotTokenUrl = "https://api.github.com/copilot_internal/v2/token";
        private const string GitHubApiVersion = "2022-11-28";

        private readonly string githubAccessToken;
        private readonly CopilotAccountType accountType;
        private readonly HttpClient httpClient;
        private Timer refreshTimer;
        private CopilotToken currentToken;

        public event EventHandler<CopilotToken> TokenRefreshed;

        /// <summary>
        /// Initializes a new instance of the GitHubCopilotTokenProvider.
        /// </summary>
        public GitHubCopilotTokenProvider(string githubAccessToken, CopilotAccountType accountType = CopilotAccountType.Individual)
        {
            if (string.IsNullOrWhiteSpace(githubAccessToken))
                throw new ArgumentException("GitHub access token is required.", nameof(githubAccessToken));

            this.githubAccessToken = githubAccessToken;
            this.accountType = accountType;
            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// Gets a Copilot token by exchanging the GitHub access token.
        /// </summary>
        public async Task<CopilotToken> GetCopilotTokenAsync(CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, CopilotTokenUrl);
            request.Headers.Add("Authorization", $"Bearer {githubAccessToken}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "MSBuildStructuredLogViewer");
            request.Headers.Add("X-GitHub-Api-Version", GitHubApiVersion);

            var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<CopilotToken>(json);

            // Extract base URL from token
            token.BaseUrl = ExtractBaseUrlFromToken(token.Token, accountType);

            currentToken = token;
            return token;
        }

        /// <summary>
        /// Extracts the base URL from the Copilot token's proxy-ep parameter.
        /// Token format: tid=...;exp=...;proxy-ep=proxy.individual.githubcopilot.com;...
        /// </summary>
        private string ExtractBaseUrlFromToken(string token, CopilotAccountType accountType)
        {
            // Try to extract from proxy-ep parameter
            var match = Regex.Match(token, @"proxy-ep=([^;]+)");
            if (match.Success)
            {
                var proxyHost = match.Groups[1].Value;
                // Convert proxy.X.githubcopilot.com to api.X.githubcopilot.com
                var apiHost = proxyHost.Replace("proxy.", "api.");
                return $"https://{apiHost}";
            }

            // Fallback based on account type
            return accountType switch
            {
                CopilotAccountType.Business => "https://api.business.githubcopilot.com",
                CopilotAccountType.Enterprise => "https://api.enterprise.githubcopilot.com",
                _ => "https://api.individual.githubcopilot.com"
            };
        }

        /// <summary>
        /// Starts automatic token refresh before expiration.
        /// </summary>
        public void StartAutoRefresh(CopilotToken initialToken)
        {
            currentToken = initialToken;
            ScheduleNextRefresh();
        }

        private void ScheduleNextRefresh()
        {
            if (currentToken == null)
                return;

            // Refresh 60 seconds before the suggested refresh time
            var refreshIn = currentToken.RefreshIn - 60;
            if (refreshIn < 60)
                refreshIn = 60; // Minimum 1 minute

            refreshTimer?.Dispose();
            refreshTimer = new Timer(
                async _ => await RefreshTokenAsync(),
                null,
                TimeSpan.FromSeconds(refreshIn),
                Timeout.InfiniteTimeSpan);

            System.Diagnostics.Debug.WriteLine($"Copilot token refresh scheduled in {refreshIn} seconds");
        }

        private async Task RefreshTokenAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Refreshing Copilot token...");
                var newToken = await GetCopilotTokenAsync();
                currentToken = newToken;

                TokenRefreshed?.Invoke(this, newToken);
                System.Diagnostics.Debug.WriteLine("Copilot token refreshed successfully");

                // Schedule next refresh
                ScheduleNextRefresh();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to refresh Copilot token: {ex.Message}");
                // Try again in 5 minutes
                refreshTimer?.Dispose();
                refreshTimer = new Timer(
                    async _ => await RefreshTokenAsync(),
                    null,
                    TimeSpan.FromMinutes(5),
                    Timeout.InfiniteTimeSpan);
            }
        }

        /// <summary>
        /// Gets the current Copilot token.
        /// </summary>
        public CopilotToken GetCurrentToken() => currentToken;

        public void Dispose()
        {
            refreshTimer?.Dispose();
            httpClient?.Dispose();
        }
    }
}
