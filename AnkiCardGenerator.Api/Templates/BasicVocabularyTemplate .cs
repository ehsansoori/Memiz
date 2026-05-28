using AnkiCardGenerator.Api.DTOs;
using AnkiCardGenerator.Api.Models;
using System.Text;
using System.Text.Json;

namespace AnkiCardGenerator.Api.Templates
{
    public class BasicVocabularyTemplate : ICardTemplate
    {
        public string Name => "basic-vocabulary";

        public CardBackDto Format(
            string input,
            DictionaryEntry dictionary,
            AiGeneratedContent aiContent,
            GenerateCardsRequestDto request)
        {
            var result = new CardBackDto
            {
                Word = input,
                Meaning = dictionary.Meaning,
                Phonetic = dictionary.Phonetic,
                PartOfSpeech = dictionary.PartOfSpeech
            };


            if (string.IsNullOrWhiteSpace(aiContent.Content))
            {
                return result;
            }
            using var document = JsonDocument.Parse(aiContent.Content);
            var root = document.RootElement;

            if (root.TryGetProperty("phonetic", out var phonetic))
            {
                result.Phonetic = phonetic.GetString();
            }

            if (root.TryGetProperty("partOfSpeech", out var partOfSpeech))
            {
                result.PartOfSpeech = partOfSpeech.GetString();
            }

            if (root.TryGetProperty("targetMeaning", out var targetMeaning))
            {
                result.TargetMeaning = targetMeaning.GetString();
            }

            if (root.TryGetProperty("englishMeaning", out var englishMeaning))
            {
                result.EnglishMeaning = englishMeaning.GetString();
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
                        Translation = example.TryGetProperty("translation", out var translation)
                            ? translation.GetString()
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
                result.Phonetic = null;
                result.TargetMeaning = null;
                result.EnglishMeaning = null;
                result.Examples.Clear();
            }

            return result;

        }
    
}
}