namespace NetConsoleWalsRiskJira.Shared
{
    // Data model for Jira ticket
    public class JiraTicket
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string WorkTeam { get; set; }
        public string FeatureArea { get; set; }
        public Dictionary<string, object> AdditionalFields { get; set; } = new();
    }

    // Configuration for which fields to embed and which to use as direct features
    public class ModelConfig
    {
        public List<string> EmbeddingFields { get; set; } = new();
        public List<string> DirectFields { get; set; } = new();
        public string EmbeddingEndpointUrl { get; set; }
    }

    // Model input for ML.NET
    public class ModelInput
    {
        public float[] Features { get; set; }
        public float Risk { get; set; } // 0-10
        public float Effort { get; set; } // in hours
    }

    // Model output
    public class ModelOutput
    {
        public float Risk { get; set; }
        public float Effort { get; set; }
    }
}
