using System.ComponentModel.DataAnnotations;

namespace CodeReviewerAI.Services.Gemini
{
    public class GeminiOptions
    {
        [Required]
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
    }
}
