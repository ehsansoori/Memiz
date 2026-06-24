namespace Memiz.Api.DTOs
{
    public class PronunciationRequestDto
    {
        public List<string> Accents { get; set; } = new() { "US", "UK" };
        //public bool IncludePhonetic { get; set; } = true;
        //public bool IncludeAudio { get; set; } = false;
    }
}
