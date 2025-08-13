using NetConsoleWalsRiskJira.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace NetConsoleWalsRiskJira
{
    public static class FeatureBuilder
    {
        public static async Task<float[]> BuildFeaturesAsync(JiraTicket ticket, ModelConfig config)
        {
            var features = new List<float>();
            // 1. Embed configured text fields
            foreach (var field in config.EmbeddingFields)
            {
                string value = field switch
                {
                    "Title" => ticket.Title,
                    "Description" => ticket.Description,
                    _ => ticket.AdditionalFields.TryGetValue(field, out var v) ? v?.ToString() : null
                };
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var embedding = await EmbeddingService.GetEmbeddingAsync(config.EmbeddingEndpointUrl, value);
                    features.AddRange(embedding);
                }
            }
            // 2. Add direct fields as single float values
            foreach (var field in config.DirectFields)
            {
                object value = field switch
                {
                    "WorkTeam" => ticket.WorkTeam,
                    "FeatureArea" => ticket.FeatureArea,
                    _ => ticket.AdditionalFields.TryGetValue(field, out var v) ? v : null
                };
                features.Add(ConvertToFloat(value));
            }
            return features.ToArray();
        }

        private static float ConvertToFloat(object value)
        {
            if (value == null) return 0f;
            if (value is float f) return f;
            if (value is int i) return i;
            if (float.TryParse(value.ToString(), out var result)) return result;
            // Simple hash for categorical
            return value.ToString().GetHashCode() % 1000 / 1000f;
        }
    }
}
