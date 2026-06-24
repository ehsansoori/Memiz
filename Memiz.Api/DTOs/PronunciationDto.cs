namespace Memiz.Api.DTOs
{
    public class PronunciationDto
    {
        public string Accent { get; set; } = string.Empty; // e.g. "us", "uk"
        public string? Phonetic { get; set; }
        // URL to audio resource or null when not available
        public string? AudioUrl { get; set; }
    }
}
