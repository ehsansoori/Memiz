using Memiz.Api.Interfaces;

namespace Memiz.Api.Prompts
{
    //its not use any more just keep it as an example of a simple prompt
    public class ExampleOnlyPrompt
    {

        public string Name => "example-only";

        public string Build(string input, string domain, string targetLanguage)
        {
            return $"""
        You are helping generate Anki card study content.

        Input: {input}
        Domain: {domain}
        Target language: {targetLanguage}

        Write one short, natural example sentence for this input.
        Keep it concise.
        """;
        }
    }
}
