using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StructuredLogViewer.LLM.GitHubCopilot.Models;

namespace StructuredLogViewer.LLM.GitHubCopilot.Authentication
{
    /// <summary>
    /// Implements GitHub OAuth Device Code Flow for authentication.
    /// See: https://docs.github.com/en/apps/oauth-apps/building-oauth-apps/authorizing-oauth-apps#device-flow
    /// </summary>
    public class GitHubDeviceFlowAuthenticator
    {
        private const string GitHubClientId = "Iv1" + "." + "b507" + "a08c87ecfe98";
        private const string GitHubDeviceCodeUrl = "https://github.com/login/device/code";
        private const string GitHubAccessTokenUrl = "https://github.com/login/oauth/access_token";
        private const string GitHubUserApiUrl = "https://api.github.com/user";

        private readonly Action<string, string> deviceCodeCallback;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the GitHubDeviceFlowAuthenticator.
        /// </summary>
        /// <param name="deviceCodeCallback">Callback invoked with (userCode, verificationUrl) to display to user.</param>
        public GitHubDeviceFlowAuthenticator(Action<string, string> deviceCodeCallback)
        {
            this.deviceCodeCallback = deviceCodeCallback ?? throw new ArgumentNullException(nameof(deviceCodeCallback));
            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// Authenticates user via device code flow.
        /// </summary>
        public async Task<string> AuthenticateAsync(CancellationToken cancellationToken = default)
        {
            // Step 1: Request device code
            var deviceCodeResponse = await RequestDeviceCodeAsync(cancellationToken);

            // Step 2: Display device code to user
            deviceCodeCallback(deviceCodeResponse.UserCode, deviceCodeResponse.VerificationUri);

            // Step 3: Poll for access token
            var accessToken = await PollForAccessTokenAsync(deviceCodeResponse, cancellationToken);

            // Step 4: Verify token works
            var isValid = await VerifyTokenAsync(accessToken, cancellationToken);
            if (!isValid)
            {
                throw new InvalidOperationException("Obtained token is not valid.");
            }

            return accessToken;
        }

        /// <summary>
        /// Requests a device code from GitHub.
        /// </summary>
        private async Task<DeviceCodeResponse> RequestDeviceCodeAsync(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, GitHubDeviceCodeUrl);
            request.Headers.Add("Accept", "application/json");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", GitHubClientId),
                new KeyValuePair<string, string>("scope", "read:user")
            });
            request.Content = content;

            var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DeviceCodeResponse>(json);
        }

        /// <summary>
        /// Polls GitHub for access token after user authorizes.
        /// </summary>
        private async Task<string> PollForAccessTokenAsync(DeviceCodeResponse deviceCode, CancellationToken cancellationToken)
        {
            var expiresAt = DateTimeOffset.UtcNow.AddSeconds(deviceCode.ExpiresIn);
            var intervalMs = deviceCode.Interval * 1000;

            while (DateTimeOffset.UtcNow < expiresAt)
            {
                await Task.Delay(intervalMs, cancellationToken);

                var request = new HttpRequestMessage(HttpMethod.Post, GitHubAccessTokenUrl);
                request.Headers.Add("Accept", "application/json");

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", GitHubClientId),
                    new KeyValuePair<string, string>("device_code", deviceCode.DeviceCode),
                    new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:device_code")
                });
                request.Content = content;

                var response = await httpClient.SendAsync(request, cancellationToken);
                var json = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(json);

                if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    System.Diagnostics.Debug.WriteLine($"Received access token (first 10 chars): {tokenResponse.AccessToken.Substring(0, Math.Min(10, tokenResponse.AccessToken.Length))}...");
                    System.Diagnostics.Debug.WriteLine($"Token type: {tokenResponse.TokenType}");
                    System.Diagnostics.Debug.WriteLine($"Scope: {tokenResponse.Scope}");
                    return tokenResponse.AccessToken;
                }

                if (!string.IsNullOrEmpty(tokenResponse.Error))
                {
                    if (tokenResponse.Error == "authorization_pending")
                    {
                        // User hasn't authorized yet, continue polling
                        continue;
                    }
                    else if (tokenResponse.Error == "slow_down")
                    {
                        // Increase interval by 5 seconds
                        intervalMs += 5000;
                        continue;
                    }
                    else if (tokenResponse.Error == "expired_token")
                    {
                        throw new InvalidOperationException("Device code expired. Please try again.");
                    }
                    else if (tokenResponse.Error == "access_denied")
                    {
                        throw new InvalidOperationException("User denied authorization.");
                    }
                    else
                    {
                        throw new InvalidOperationException($"GitHub returned error: {tokenResponse.Error} - {tokenResponse.ErrorDescription}");
                    }
                }
            }

            throw new TimeoutException("Device code expired before user authorized.");
        }

        /// <summary>
        /// Verifies that a GitHub token is valid.
        /// </summary>
        public async Task<bool> VerifyTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, GitHubUserApiUrl);
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "MSBuildStructuredLogViewer");

                var response = await httpClient.SendAsync(request, cancellationToken);
                
                // Debug logging for troubleshooting
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"VerifyTokenAsync failed: {response.StatusCode}");
                    System.Diagnostics.Debug.WriteLine($"Response body: {responseBody}");
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Gets the GitHub user information for a token.
        /// </summary>
        public async Task<GitHubUser> GetUserAsync(string token, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, GitHubUserApiUrl);
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "MSBuildStructuredLogViewer");

            var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GitHubUser>(json);
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
