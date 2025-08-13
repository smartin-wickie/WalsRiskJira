
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using NetConsoleWalsRiskJira.Shared;
using NetConsoleWalsRiskJira;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Risk Evaluation Application - Starting up...");
        // Load config
        var config = new ModelConfig
        {
            EmbeddingFields = new() { "Title", "Description" },
            DirectFields = new() { "WorkTeam", "FeatureArea", "Priority", "StoryPoints" },
            EmbeddingEndpointUrl = "http://localhost:11434/api/embeddings" // Example
        };
        // Load mock ticket
        var ticketJson = await File.ReadAllTextAsync("mock-jira-ticket.json");
        var ticket = JsonSerializer.Deserialize<JiraTicket>(ticketJson);
        // Build features
        var features = await FeatureBuilder.BuildFeaturesAsync(ticket, config);
        Console.WriteLine($"Feature vector length: {features.Length}");

        // Mock training data (in real use, load from dataset)
        var trainingData = new List<ModelInput>
        {
            new ModelInput { Features = features, Risk = 7.5f, Effort = 24f },
            new ModelInput { Features = features.Select(f => f * 0.9f).ToArray(), Risk = 5.0f, Effort = 16f },
            new ModelInput { Features = features.Select(f => f * 1.1f).ToArray(), Risk = 9.0f, Effort = 32f }
        };
        // Train and save model
        ModelTrainer.TrainAndSaveModel(trainingData, "wals-model");
        Console.WriteLine("Model trained and saved.");

        // Predict using the trained model
        var (risk, effort) = ModelTrainer.Predict("wals-model", features);
        Console.WriteLine($"Predicted Risk: {risk:F2} (0-10), Effort: {effort:F2} hours");
    }
}
