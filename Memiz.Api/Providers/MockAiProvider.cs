using Memiz.Api.DTOs;
using Memiz.Api.Interfaces;
using Memiz.Api.Models;

namespace Memiz.Api.Providers;

public class MockAiProvider : IAiProvider
{
    public string Name => "default";

    public AiGeneratedContent GenerateContent(string input, GenerateLanguageCardsRequestDto request)
    {
        //throw new Exception("MockAiProvider is being used");

        return new AiGeneratedContent
        {
            Content = $"Mock generated content for '{input}'",
            Provider = Name,
            TargetLanguage = request.TargetLanguage,
           // Domain = request.Domain
        };
    }
}
