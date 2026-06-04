using Memiz.Api.DTOs;
using Memiz.Api.Factories;
using Memiz.Api.Interfaces;
using Memiz.Api.Templates;

namespace Memiz.Api.Services;

public class CardService : ICardService
{
    private readonly DictionaryProviderFactory _dictionaryProviderFactory;
    private readonly AiProviderFactory _aiProviderFactory;
    private readonly TemplateFactory _templateFactory;
    public CardService(DictionaryProviderFactory dictionaryProviderFactory,
    AiProviderFactory aiProviderFactory, TemplateFactory templateFactory)
    {
        _dictionaryProviderFactory = dictionaryProviderFactory;
        _aiProviderFactory = aiProviderFactory;
        _templateFactory = templateFactory;
    }
     
    public List<CardResponseDto> GenerateCards(GenerateCardsRequestDto request)
    {
        var result = new List<CardResponseDto>();

        // select providers
        var dictionaryProvider = _dictionaryProviderFactory.GetProvider(request.DictionaryProvider);
        var aiProvider = _aiProviderFactory.GetProvider(request.AiProvider);

        // select template
        var template = _templateFactory.GetTemplate(request.TemplateName);

        foreach (var input in request.Inputs)
        {
            var dictionaryEntry = dictionaryProvider.GetEntry(
                input,
                request.SourceLanguage,
                request.TargetLanguage);

            var aiContent = aiProvider.GenerateContent(
                input,
              request);

           
            var back = template.Format(
             input,
             dictionaryEntry,
             aiContent,
             request);

            result.Add(new CardResponseDto
            {
                Front = input,
                Back = back 
            });
        }

        return result;
    }
}