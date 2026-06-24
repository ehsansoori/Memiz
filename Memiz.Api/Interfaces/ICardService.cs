using Memiz.Api.DTOs;

namespace Memiz.Api.Interfaces
{
    public interface ICardService
    {
        List<CardResponseDto> GenerateLanguageCards(GenerateLanguageCardsRequestDto request);
    }
}
