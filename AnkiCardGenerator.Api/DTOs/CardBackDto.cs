namespace AnkiCardGenerator.Api.DTOs
{
    public class CardBackDto
    {
        public string Word { get; set; } = string.Empty;
        public string? Meaning { get; set; }
        public string? Phonetic { get; set; }
        public string? PartOfSpeech { get; set; }
        public string? TargetMeaning { get; set; }
        public string? EnglishMeaning { get; set; }
        public List<ExampleDto> Examples { get; set; } = new();
        // Indicates whether the input is a valid / confidently recognized word or phrase.
        public bool IsValid { get; set; } = true;

        // When IsValid is false, the AI may return up to 5 suggestions for possible intended words/phrases.
        public List<string> Suggestions { get; set; } = new();
    }
}
