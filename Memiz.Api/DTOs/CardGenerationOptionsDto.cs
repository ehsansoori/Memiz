namespace Memiz.Api.DTOs
{
    public class CardGenerationOptionsDto
    {
        public bool includeTranslation { get; set; } = true;
        //public bool includeDefinition { get; set; } = true;
        public bool IncludePartOfSpeech { get; set; } = true;
        public int ExampleCount { get; set; } = 1;
        public bool IncludeExampleTranslations { get; set; } = true;
        public List<string> Pronunciations { get; set; } = []; //"en","fa"
        //public string Tone { get; set; } = "neutral";
        //public string DifficultyLevel { get; set; } = "beginner";
    }
}
