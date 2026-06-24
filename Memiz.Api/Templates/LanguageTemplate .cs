using Memiz.Api.DTOs;
using Memiz.Api.Models;
using System.Text;
using System.Text.Json;

namespace Memiz.Api.Templates
{
    public class LanguageTemplate : ICardTemplate
    {
        public string Name => "LanguageTemplate";

        public CardBackDto Format(
            string input,
            //DictionaryEntry dictionary,
            AiGeneratedContent aiContent,
            GenerateLanguageCardsRequestDto request)
        {
            var result = new CardBackDto
            {
                input = input,
                //Meaning = dictionary.Meaning,
                //Phonetic = dictionary.Phonetic,
                //PartOfSpeech = dictionary.PartOfSpeech
            };


            if (string.IsNullOrWhiteSpace(aiContent.Content))
            {
                return result;
            }
            using var document = JsonDocument.Parse(aiContent.Content);
            var root = document.RootElement;

            if (root.TryGetProperty("translation", out var translation))
            {
                result.Translation = translation.GetString();
            }

            //if (root.TryGetProperty("definition", out var definition))
            //{
            //    result.Definition = definition.GetString();
            //}

        

            if (root.TryGetProperty("partOfSpeech", out var partOfSpeech) &&
                partOfSpeech.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in partOfSpeech.EnumerateArray())
                {
                    var str = item.GetString();
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        result.PartOfSpeech.Add(str);
                    }
                }
            }

            if (root.TryGetProperty("pronunciations", out var pronunciations) &&
              pronunciations.ValueKind == JsonValueKind.Array)
            {
                foreach (var pronunciation in pronunciations.EnumerateArray())
                {
                    var item = new PronunciationDto
                    {
                        Accent = pronunciation.TryGetProperty("accent", out var accent)
                            ? accent.GetString() ?? string.Empty
                            : string.Empty,
                        Phonetic = pronunciation.TryGetProperty("phonetic", out var phonetic)
                            ? phonetic.GetString()
                            : null
                    };

                    result.Pronunciations.Add(item);
                }
            }
                if (root.TryGetProperty("examples", out var examples) &&
                examples.ValueKind == JsonValueKind.Array)
            {
                foreach (var example in examples.EnumerateArray())
                {
                    var item = new ExampleDto
                    {
                        Sentence = example.TryGetProperty("sentence", out var sentence)
                            ? sentence.GetString() ?? string.Empty
                            : string.Empty,
                        Translation = example.TryGetProperty("translation", out var exampleTranslation)
                            ? exampleTranslation.GetString()
                            : null
                    };

                    result.Examples.Add(item);
                }
            }

            // handle isValid and suggestions
            if (root.TryGetProperty("isValid", out var isValidProp) &&
                isValidProp.ValueKind == JsonValueKind.False)
            {
                // mark invalid and clear any AI-provided fields to avoid fabricated content
                // The CardBackDto supports IsValid and Suggestions (added in DTO)
                result.IsValid = false;

                if (root.TryGetProperty("suggestions", out var suggestions) &&
                    suggestions.ValueKind == JsonValueKind.Array)
                {
                    foreach (var s in suggestions.EnumerateArray())
                    {
                        var str = s.GetString();
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            result.Suggestions.Add(str);
                        }
                    }
                }

                // ensure we don't keep AI-filled phonetic/meanings/examples when invalid
                //result.Phonetic = null;
                //result.TargetMeaning = null;
                //result.EnglishMeaning = null;
                result.Examples.Clear();
            }

            return result;

        }
    
}
}