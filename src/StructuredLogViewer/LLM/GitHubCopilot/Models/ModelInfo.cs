using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StructuredLogViewer.LLM.GitHubCopilot.Models
{
    /// <summary>
    /// Response from models API.
    /// </summary>
    public class ModelsResponse
    {
        [JsonPropertyName("data")]
        public List<ModelInfo> Data { get; set; }
    }

    /// <summary>
    /// Information about an available model.
    /// </summary>
    public class ModelInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("model_picker_enabled")]
        public bool ModelPickerEnabled { get; set; }

        [JsonPropertyName("policy")]
        public ModelPolicy Policy { get; set; }
    }

    /// <summary>
    /// Model policy information.
    /// </summary>
    public class ModelPolicy
    {
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
