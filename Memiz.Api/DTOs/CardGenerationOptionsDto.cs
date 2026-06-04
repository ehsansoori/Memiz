namespace Memiz.Api.DTOs
{
    public class CardGenerationOptionsDto
    {
        public bool IncludePhonetic { get; set; } = true;
        public bool IncludePartOfSpeech { get; set; } = true;
        public bool IncludeTargetMeaning { get; set; } = true;
        public bool IncludeEnglishMeaning { get; set; } = false;
        public int ExampleCount { get; set; } = 1;
        public bool IncludeExampleTranslations { get; set; } = true;
        public string Tone { get; set; } = "neutral";
        public string DifficultyLevel { get; set; } = "beginner";
    }
}
