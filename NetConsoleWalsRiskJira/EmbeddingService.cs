using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetConsoleWalsRiskJira
{
    public static class EmbeddingService
    {
        public static async Task<float[]> GetEmbeddingAsync(string endpointUrl, string text)
        {
            using var client = new HttpClient();
            var payload = new { input = text };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpointUrl, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            // Assume response: { "embedding": [ ... ] }
            using var doc = JsonDocument.Parse(json);
            var arr = doc.RootElement.GetProperty("embedding");
            var result = new float[arr.GetArrayLength()];
            for (int i = 0; i < arr.GetArrayLength(); i++)
                result[i] = arr[i].GetSingle();
            return result;
        }
    }
}
