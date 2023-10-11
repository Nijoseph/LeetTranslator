using Newtonsoft.Json;
using System.Text;

namespace LeetTranslator.ApiIntegration
{

    public class FunTranslationsService : IFunTranslationsService
    {
        private readonly string apiKey;
        private readonly HttpClient httpClient;
        private readonly string apiUrl;

        public FunTranslationsService(string apiKey, string apiUrl)
        {
            this.apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            httpClient = new HttpClient();
            this.apiUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
        }

        public async Task<FunTranslationsResponse> Translate(string text)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Add("X-Funtranslations-Api-Secret", apiKey);

                string jsonPayload = $"{{\"text\": \"{text}\"}}";
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    FunTranslationsResponse translationResponse = JsonConvert.DeserializeObject<FunTranslationsResponse>(responseContent);

                    return translationResponse;
                }
                else
                {
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }

}
