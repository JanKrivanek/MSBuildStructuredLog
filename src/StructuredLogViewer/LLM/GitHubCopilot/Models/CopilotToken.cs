using System;
using System.Text.Json.Serialization;

namespace StructuredLogViewer.LLM.GitHubCopilot.Models
{
    /// <summary>
    /// GitHub Copilot token with metadata.
    /// </summary>
    public class CopilotToken
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expires_at")]
        public long ExpiresAtUnix { get; set; }

        [JsonPropertyName("refresh_in")]
        public int RefreshIn { get; set; }

        [JsonIgnore]
        public DateTimeOffset ExpiresAt
        {
            get => DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnix);
            set => ExpiresAtUnix = value.ToUnixTimeSeconds();
        }

        [JsonIgnore]
        public string BaseUrl { get; set; }

        [JsonIgnore]
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt.AddMinutes(-5);
    }

    /// <summary>
    /// GitHub user information.
    /// </summary>
    public class GitHubUser
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
