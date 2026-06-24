using System.Text;
using System.Linq;
using System.Collections.Generic;
using Memiz.Api.DTOs;

namespace Memiz.Api.Factories
{
    public class AiPromptFactory
    {
        public string BuildPrompt(string input, GenerateLanguageCardsRequestDto request)
        {
            var options = request.Options ?? new CardGenerationOptionsDto();
            var exampleCount = Math.Max(0, options.ExampleCount);

            var accents = options.Pronunciations?.Any() == true
                ? string.Join(", ", options.Pronunciations)
                : "none";

            var sb = new StringBuilder();

            sb.AppendLine("You are a strict JSON generator for language flashcards.");
            sb.AppendLine("Return ONLY a single valid JSON object.");
            sb.AppendLine("Do not return markdown.");
            sb.AppendLine("Do not return explanations.");
            sb.AppendLine("Do not return code fences.");
            sb.AppendLine();

            sb.AppendLine($"Input: {input}");
            sb.AppendLine($"Source Language: {request.SourceLanguage}");
            sb.AppendLine($"Target Language: {request.TargetLanguage}");
            sb.AppendLine($"Example Count: {exampleCount}");
            sb.AppendLine($"Requested Pronunciation Accents: {accents}");
            sb.AppendLine();

            sb.AppendLine("JSON Schema:");

            sb.AppendLine(@"
{
  ""input"": ""string"",
  ""translation"": ""string | null"",
  ""pronunciations"": [
    {
      ""accent"": ""string"",
      ""phonetic"": ""string""
    }
  ],
  ""partOfSpeech"": [""string""],
  ""examples"": [
    {
      ""sentence"": ""string"",
      ""translation"": ""string""
    }
  ],
  ""isValid"": true,
  ""suggestions"": []
}
");

            sb.AppendLine("Rules:");

            sb.AppendLine("- If the input is a valid word or phrase:");
            sb.AppendLine("- isValid must be true.");
            sb.AppendLine("- translation must not be null.");
            sb.AppendLine("- partOfSpeech must contain at least one value.");
            //part Of Speech roles
            sb.AppendLine("Part Of Speech Rules:");
            sb.AppendLine("- Use the most specific part of speech possible.");
            sb.AppendLine("- Do not use generic classifications when a more specific classification exists.");
            sb.AppendLine("- Examples:");
            sb.AppendLine("- give up -> phrasal verb");
            sb.AppendLine("- take off -> phrasal verb");
            sb.AppendLine("- look after -> phrasal verb");
            sb.AppendLine("- break down -> phrasal verb");

            sb.AppendLine("- kick the bucket -> idiom");
            sb.AppendLine("- piece of cake -> idiom");

            sb.AppendLine("- make a decision -> collocation");
            sb.AppendLine("- heavy rain -> collocation");

            sb.AppendLine("- If the input is a phrasal verb, return:");
            sb.AppendLine(@"  ""partOfSpeech"": [""phrasal verb""]");

            sb.AppendLine("- If the input is an idiom, return:");
            sb.AppendLine(@"  ""partOfSpeech"": [""idiom""]");

            sb.AppendLine("- If the input is a collocation, return:");
            sb.AppendLine(@"  ""partOfSpeech"": [""collocation""]");

            sb.AppendLine("- Do not classify phrasal verbs as 'verb'.");
            sb.AppendLine("- Do not classify idioms as 'phrase'.");
            sb.AppendLine();
            //end part Of Speech roles
            sb.AppendLine($"- examples must contain exactly {exampleCount} items.");
            sb.AppendLine("- suggestions must be an empty array.");
            sb.AppendLine();

            sb.AppendLine("- If the input is misspelled:");
            sb.AppendLine("- isValid must be false.");
            sb.AppendLine("- translation must be null.");
            sb.AppendLine("- examples must be empty.");
            sb.AppendLine("- pronunciations must be empty.");
            sb.AppendLine("- partOfSpeech must be empty.");
            sb.AppendLine("- suggestions must contain only real dictionary-valid corrections.");
            sb.AppendLine("- suggestions must not contain the original input.");
            sb.AppendLine("- suggestions must not contain invented words.");
            sb.AppendLine("- return only the single most likely correction.");
            sb.AppendLine();

            sb.AppendLine("- If the input is complete gibberish:");
            sb.AppendLine("- isValid must be false.");
            sb.AppendLine("- suggestions must be empty.");
            sb.AppendLine();

            sb.AppendLine("Pronunciations:");

            sb.AppendLine("- Generate one pronunciation object for each requested accent.");
            sb.AppendLine("- Only return requested accents.");
            sb.AppendLine("- Each pronunciation contains only accent and phonetic.");
            sb.AppendLine();

            sb.AppendLine("Examples:");

            sb.AppendLine("- Examples must be natural.");
            sb.AppendLine("- Examples must match the meaning.");
            sb.AppendLine("- Examples must be grammatically correct.");
            sb.AppendLine("- Never invent fake usages.");
            sb.AppendLine();

            sb.AppendLine("Validation:");

            sb.AppendLine("- NEVER return isValid=true with empty examples.");
            sb.AppendLine("- NEVER return isValid=true with suggestions.");
            sb.AppendLine("- NEVER return isValid=false with examples.");
            sb.AppendLine("- NEVER return suggestions containing the original input.");
            sb.AppendLine();

            sb.AppendLine("Valid Example:");

            sb.AppendLine(@"
{
  ""input"": ""apple"",
  ""translation"": ""سیب"",
  ""pronunciations"": [
    {
      ""accent"": ""us"",
      ""phonetic"": ""/ˈæpəl/""
    }
  ],
  ""partOfSpeech"": [""noun""],
  ""examples"": [
    {
      ""sentence"": ""I eat an apple every day."",
      ""translation"": ""من هر روز یک سیب می‌خورم.""
    }
  ],
  ""isValid"": true,
  ""suggestions"": []
}
");
            sb.AppendLine("Phrasal Verb Example:");

            sb.AppendLine(
            @"{
  ""input"": ""give up"",
  ""translation"": ""تسلیم شدن"",
  ""pronunciations"": [
    {
      ""accent"": ""us"",
      ""phonetic"": ""/ɡɪv ʌp/""
    }
  ],
  ""partOfSpeech"": [""phrasal verb""],
  ""examples"": [
    {
      ""sentence"": ""Don't give up on your dreams."",
      ""translation"": ""از رویاهایت دست نکش.""
    }
  ],
  ""isValid"": true,
  ""suggestions"": []
}
");

            sb.AppendLine("Misspelling Example:");

            sb.AppendLine(@"
{
  ""input"": ""enviroment"",
  ""translation"": null,
  ""pronunciations"": [],
  ""partOfSpeech"": [],
  ""examples"": [],
  ""isValid"": false,
  ""suggestions"": [
    ""environment""
  ]
}
");

            sb.AppendLine("Gibberish Example:");

            sb.AppendLine(@"
{
  ""input"": ""sdkfjhskdfh"",
  ""translation"": null,
  ""pronunciations"": [],
  ""partOfSpeech"": [],
  ""examples"": [],
  ""isValid"": false,
  ""suggestions"": []
}
");

            return sb.ToString();
        }
    }

}