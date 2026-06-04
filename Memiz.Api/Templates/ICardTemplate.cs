using Memiz.Api.DTOs;
using Memiz.Api.Models;

namespace Memiz.Api.Templates
{
    public interface ICardTemplate
    {
        string Name { get; }

        CardBackDto Format(
            string input,
            DictionaryEntry dictionary,
            AiGeneratedContent aiContent,
            GenerateCardsRequestDto request);
    }
}
