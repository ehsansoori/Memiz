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
    private readonly IConfiguration _configuration;
    public CardService(DictionaryProviderFactory dictionaryProviderFactory,
    AiProviderFactory aiProviderFactory, TemplateFactory templateFactory, IConfiguration configuration)
    {
        _dictionaryProviderFactory = dictionaryProviderFactory;
        _aiProviderFactory = aiProviderFactory;
        _templateFactory = templateFactory;
        _configuration = configuration;


    }

    public List<CardResponseDto> GenerateLanguageCards(GenerateLanguageCardsRequestDto request)
    {
        var result = new List<CardResponseDto>();

        // select providers
        // var dictionaryProvider = _dictionaryProviderFactory.GetProvider(request.DictionaryProvider);

        var providerName = _configuration["Ai:DefaultProvider"];

        var aiProvider =
            _aiProviderFactory.GetProvider(providerName!);

        // select template
        var template = _templateFactory.GetTemplate("LanguageTemplate");

        foreach (var input in request.Inputs)
        {
            //var dictionaryEntry = dictionaryProvider.GetEntry(
            //    input,
            //    request.SourceLanguage,
            //    request.TargetLanguage);

            var aiContent = aiProvider.GenerateContent(
                input,
              request);

           
            var back = template.Format(
             input,
             //dictionaryEntry,
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