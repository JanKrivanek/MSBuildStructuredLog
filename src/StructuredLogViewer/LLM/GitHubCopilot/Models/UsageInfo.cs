using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StructuredLogViewer.LLM.GitHubCopilot.Models
{
    /// <summary>
    /// GitHub Copilot usage and quota information.
    /// </summary>
    public class CopilotUsageInfo
    {
        [JsonPropertyName("access_type_sku")]
        public string AccessTypeSku { get; set; }

        [JsonPropertyName("copilot_plan")]
        public string CopilotPlan { get; set; }

        [JsonPropertyName("quota_reset_date")]
        public string QuotaResetDate { get; set; }

        [JsonPropertyName("quota_snapshots")]
        public QuotaSnapshots QuotaSnapshots { get; set; }

        [JsonPropertyName("chat_enabled")]
        public bool ChatEnabled { get; set; }

        [JsonPropertyName("assigned_date")]
        public string AssignedDate { get; set; }
    }

    /// <summary>
    /// Quota snapshots for different services.
    /// </summary>
    public class QuotaSnapshots
    {
        [JsonPropertyName("chat")]
        public QuotaDetail Chat { get; set; }

        [JsonPropertyName("completions")]
        public QuotaDetail Completions { get; set; }

        [JsonPropertyName("premium_interactions")]
        public QuotaDetail PremiumInteractions { get; set; }
    }

    /// <summary>
    /// Detailed quota information for a service.
    /// </summary>
    public class QuotaDetail
    {
        [JsonPropertyName("entitlement")]
        public int Entitlement { get; set; }

        [JsonPropertyName("remaining")]
        public int Remaining { get; set; }

        [JsonPropertyName("percent_remaining")]
        public double PercentRemaining { get; set; }

        [JsonPropertyName("unlimited")]
        public bool Unlimited { get; set; }

        [JsonPropertyName("overage_count")]
        public int OverageCount { get; set; }

        [JsonPropertyName("overage_permitted")]
        public bool OveragePermitted { get; set; }

        [JsonPropertyName("quota_id")]
        public string QuotaId { get; set; }

        [JsonPropertyName("quota_remaining")]
        public int QuotaRemaining { get; set; }
    }
}
