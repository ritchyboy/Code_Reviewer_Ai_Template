using System.Text.Json.Serialization;

namespace CodeReviewerAI.Models
{
    public class ReviewResult
    {
        [JsonPropertyName("is_approved")]
        public bool IsApproved { get; set; }

        [JsonPropertyName("risk_score")]
        public int RiskScore { get; set; }

        [JsonPropertyName("risk_level")]
        public string RiskLevel { get; set; } = "Unknown";

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;

        [JsonPropertyName("markdown_review")]
        public string MarkdownReview { get; set; } = string.Empty;
    }
    
}