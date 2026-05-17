using AnkiCardGenerator.Api.DTOs;
using AnkiCardGenerator.Api.Factories;
using AnkiCardGenerator.Api.Interfaces;
using AnkiCardGenerator.Api.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AnkiCardGenerator.Api.Providers
{

    public class OpenAiProvider : IAiProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AiPromptFactory _promptFactory;


        public string Name => "openai";

        public OpenAiProvider(HttpClient httpClient, IConfiguration configuration, AiPromptFactory promptFactory)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _promptFactory = promptFactory;

        }

        public AiGeneratedContent GenerateContent(string input, GenerateCardsRequestDto request)
        {
            //throw new Exception("OpenAiProvider is being used");

            var apiKey = _configuration["OpenAI:ApiKey"];
            var model = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("OpenAI:ApiKey is missing.");
            }

            var prompt = _promptFactory.BuildPrompt(input, request);


            var requestBody = new
            {
                model,
                input = prompt
            };

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.openai.com/v1/responses");

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            using var response = _httpClient.SendAsync(httpRequest)
                .GetAwaiter()
                .GetResult();

            var responseJson = response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"OpenAI request failed: {(int)response.StatusCode} {response.ReasonPhrase}. {responseJson}");
            }

            var content = ExtractOutputText(responseJson);

            return new AiGeneratedContent
            {
                Content = content,
                Provider = Name,
                TargetLanguage = request.TargetLanguage,
                Domain = request.Domain,
            };
        }

        private static string ExtractOutputText(string json)
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (root.TryGetProperty("output_text", out var outputText))
            {
                return outputText.GetString() ?? string.Empty;
            }

            if (!root.TryGetProperty("output", out var outputItems))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var item in outputItems.EnumerateArray())
            {
                if (!item.TryGetProperty("content", out var contentItems))
                {
                    continue;
                }

                foreach (var contentItem in contentItems.EnumerateArray())
                {
                    if (contentItem.TryGetProperty("text", out var textElement))
                    {
                        builder.AppendLine(textElement.GetString());
                    }
                }
            }

            return builder.ToString().Trim();
        }
    }
}
