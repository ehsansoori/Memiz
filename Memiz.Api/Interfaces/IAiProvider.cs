using Memiz.Api.DTOs;
using Memiz.Api.Models;

namespace Memiz.Api.Interfaces
{
    public interface IAiProvider
    {
        string Name { get; }

        AiGeneratedContent GenerateContent(string input, GenerateLanguageCardsRequestDto request);

    }
}
