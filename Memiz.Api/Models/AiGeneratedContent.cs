namespace Memiz.Api.Models
{
    public class AiGeneratedContent
    {
        public string Content { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
    }
}
