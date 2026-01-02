using System;
using StructuredLogViewer.LLM.GitHubCopilot.Authentication;

namespace StructuredLogViewer.LLM.GitHubCopilot.Configuration
{
    /// <summary>
    /// Configuration for GitHub Copilot client.
    /// </summary>
    public class GitHubCopilotConfiguration
    {
        /// <summary>
        /// GitHub access token (optional if using device flow).
        /// </summary>
        public string GitHubToken { get; set; }

        /// <summary>
        /// Account type for Copilot.
        /// </summary>
        public CopilotAccountType AccountType { get; set; } = CopilotAccountType.Individual;

        /// <summary>
        /// Default model to use.
        /// </summary>
        public string DefaultModel { get; set; } = "claude-sonnet-4.5";

        /// <summary>
        /// Base URL override (optional, auto-detected from token if not provided).
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Whether to automatically refresh tokens.
        /// </summary>
        public bool AutoRefreshToken { get; set; } = true;

        /// <summary>
        /// Loads configuration from environment variables.
        /// </summary>
        public static GitHubCopilotConfiguration LoadFromEnvironment()
        {
            var config = new GitHubCopilotConfiguration();

            // Check for GitHub Copilot-specific environment variables
            var token = Environment.GetEnvironmentVariable("GITHUB_COPILOT_TOKEN");
            if (!string.IsNullOrWhiteSpace(token))
            {
                config.GitHubToken = token;
            }

            var accountTypeStr = Environment.GetEnvironmentVariable("GITHUB_COPILOT_ACCOUNT_TYPE");
            if (Enum.TryParse<CopilotAccountType>(accountTypeStr, true, out var accountType))
            {
                config.AccountType = accountType;
            }

            var model = Environment.GetEnvironmentVariable("GITHUB_COPILOT_MODEL");
            if (!string.IsNullOrWhiteSpace(model))
            {
                config.DefaultModel = model;
            }

            var baseUrl = Environment.GetEnvironmentVariable("GITHUB_COPILOT_BASE_URL");
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                config.BaseUrl = baseUrl;
            }

            // Also check generic LLM environment variables as fallback
            if (string.IsNullOrWhiteSpace(config.GitHubToken))
            {
                var llmApiKey = Environment.GetEnvironmentVariable("LLM_API_KEY");
                if (!string.IsNullOrWhiteSpace(llmApiKey) && llmApiKey.StartsWith("ghp_"))
                {
                    config.GitHubToken = llmApiKey;
                }
            }

            if (string.IsNullOrWhiteSpace(config.BaseUrl))
            {
                var llmEndpoint = Environment.GetEnvironmentVariable("LLM_ENDPOINT");
                if (!string.IsNullOrWhiteSpace(llmEndpoint) &&
                    llmEndpoint.Contains("githubcopilot.com", StringComparison.OrdinalIgnoreCase))
                {
                    config.BaseUrl = llmEndpoint;
                }
            }

            return config;
        }

        /// <summary>
        /// Validates the configuration.
        /// </summary>
        public bool IsValid()
        {
            // Either we have a GitHub token, or we'll need to do device flow
            return !string.IsNullOrWhiteSpace(DefaultModel);
        }

        /// <summary>
        /// Gets a status message for the configuration.
        /// </summary>
        public string GetStatusMessage()
        {
            if (!IsValid())
            {
                return "Invalid configuration: Model name is required.";
            }

            if (string.IsNullOrWhiteSpace(GitHubToken))
            {
                return "Configuration ready. GitHub authentication required (device flow will be initiated).";
            }

            return $"Configuration ready. Using {AccountType} account with model {DefaultModel}.";
        }
    }
}
