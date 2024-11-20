using Newtonsoft.Json;
using System.Text;

namespace PdfQAProcessor_n.Services
{
    public class HuggingFaceService
    {
        private readonly string _apiKey = "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"; // Use your API Key
        private readonly HttpClient _httpClient;

        public HuggingFaceService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> AskQuestionAsync(string extractedText, string question)
        {
            var url = "https://api-inference.huggingface.co/models/distilbert-base-cased-distilled-squad"; // Question answering model

            var body = new
            {
                inputs = new
                {
                    question = question,
                    context = extractedText
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(responseString);

            // Process result if necessary (e.g., trim long responses)
            string answer = result?.answer ?? "No answer found";
            return answer.Length > 150 ? answer.Substring(0, 150) + "..." : answer; // Limit the answer length
        }
    }
}
