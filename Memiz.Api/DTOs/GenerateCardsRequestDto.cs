namespace Memiz.Api.DTOs
{
    public class GenerateCardsRequestDto
    {
        public List<string> Inputs { get; set; } = new();

        public string InputType { get; set; } = "word";

        public string SourceLanguage { get; set; } = "en";

        public string TargetLanguage { get; set; } = "fa";

        public string Domain { get; set; } = "daily conversation";

        public string TemplateName { get; set; } = "basic-vocabulary";

        public string DictionaryProvider { get; set; } = "default";

        public string AiProvider { get; set; } = "default";

        public CardGenerationOptionsDto Options { get; set; } = new();
               

    }
}
