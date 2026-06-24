namespace Memiz.Api.DTOs
{
    public class CardBackDto
    {
        public string input { get; set; } = string.Empty;
        public List<PronunciationDto> Pronunciations { get; set; } = new();
        public string? Translation { get; set; }
       // public string? Definition { get; set; }
        public List<string> PartOfSpeech { get; set; }=new();
        public List<ExampleDto> Examples { get; set; } = new();
        // Indicates whether the input is a valid / confidently recognized word or phrase.
        public bool IsValid { get; set; } = true;

        // When IsValid is false, the AI may return up to 5 suggestions for possible intended words/phrases.
        public List<string> Suggestions { get; set; } = new();
    }
}
