using System.Text;
using Memiz.Api.DTOs;

namespace Memiz.Api.Factories
{
    public class AiPromptFactory
    {
        public string BuildPrompt(string input, GenerateCardsRequestDto request)
        {
            var options = request.Options ?? new CardGenerationOptionsDto();
            var exampleCount = options.ExampleCount < 1 ? 1 : options.ExampleCount;
            var tone = string.IsNullOrWhiteSpace(options.Tone) ? "neutral" : options.Tone;
            var difficulty = string.IsNullOrWhiteSpace(options.DifficultyLevel)
                ? "beginner"
                : options.DifficultyLevel;

            var fields = new List<string>
            {
                "\"word\": \"input word or phrase\"",
                    "\"isValid\": true"


            };

            if (options.IncludePhonetic)
                fields.Add("\"phonetic\": \"phonetic transcription\"");

            if (options.IncludePartOfSpeech)
                fields.Add("\"partOfSpeech\": \"part of speech\"");

            if (options.IncludeTargetMeaning)
                fields.Add($"\"targetMeaning\": \"meaning in {request.TargetLanguage}\"");

            if (options.IncludeEnglishMeaning)
                fields.Add("\"englishMeaning\": \"meaning in English\"");

            fields.Add("\"validationMessage\": \"optional validation warning\"");

            var exampleFields = new List<string>
            {
                "\"sentence\": \"example sentence\""
            };

            if (options.IncludeExampleTranslations)
                exampleFields.Add($"\"translation\": \"translation in {request.TargetLanguage}\"");

            var sb = new StringBuilder();
            sb.AppendLine("If the word has multiple common parts of speech:");
            sb.AppendLine("- set partOfSpeech to something like \"noun, verb\"");
            sb.AppendLine("- include meanings for the important usages");
            sb.AppendLine("- generate examples that demonstrate different usages when possible");
            sb.AppendLine("You are generating structured flashcard content for an Anki card.");
            sb.AppendLine("Return ONLY valid JSON.");
            sb.AppendLine("Do not include markdown or explanation.");
            sb.AppendLine();
            sb.AppendLine($"Input: {input}");
            sb.AppendLine($"Source language: {request.SourceLanguage}");
            sb.AppendLine($"Target language: {request.TargetLanguage}");
            sb.AppendLine($"Domain: {request.Domain}");
            sb.AppendLine($"Tone: {tone}");
            sb.AppendLine($"Difficulty level: {difficulty}");
            sb.AppendLine($"Generate exactly {exampleCount} examples.");
            sb.AppendLine();
            sb.AppendLine("Use this JSON shape:");
            sb.AppendLine("Return ONLY raw valid JSON.");
            sb.AppendLine("If the word has multiple common usages or parts of speech:");
            sb.AppendLine("- include the most important usages");
            sb.AppendLine("- generate separate meanings and examples for each usage");
            sb.AppendLine("- examples must clearly match their corresponding part of speech");
            sb.AppendLine("If the input is not a valid or meaningful word/phrase in the selected language:");
            sb.AppendLine("- set \"isValid\" to false");
            sb.AppendLine("- leave generated fields empty");
            sb.AppendLine("- set \"validationMessage\" with a short warning");
            sb.AppendLine("- do not invent meanings");
            sb.AppendLine("- do not invent examples");
            sb.AppendLine("- still return valid JSON");
            sb.AppendLine("Do not wrap the response in markdown.");
            sb.AppendLine("Do not use ```json.");
            sb.AppendLine("Do not write explanations.");
            sb.AppendLine("The response must start with { and end with }.");
            sb.AppendLine("All property names and string values must use double quotes.");
            sb.AppendLine("{");

            foreach (var field in fields)
            {
                sb.AppendLine($"  {field},");
            }
            sb.AppendLine("  \"examples\": [");
            sb.AppendLine($"    {{ {string.Join(", ", exampleFields)} }}");
            sb.AppendLine("  ],");
            sb.AppendLine("  \"isValid\": true,");
            sb.AppendLine("  \"suggestions\": []");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("The examples array must contain exactly the requested number of items.");
            sb.AppendLine("If the input is NOT a real or confidently recognized word/phrase then:");
            sb.AppendLine("- set \"isValid\" to false");
            sb.AppendLine("- DO NOT invent meanings, phonetics, parts of speech or examples");
            sb.AppendLine("- return up to 5 possible suggestions for intended words/phrases in the \"suggestions\" array");
            sb.AppendLine("- provide an empty \"examples\" array and leave other optional fields empty or null");

            return sb.ToString();
        }
    }
}
