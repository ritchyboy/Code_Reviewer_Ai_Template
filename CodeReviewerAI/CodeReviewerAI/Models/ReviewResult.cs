namespace CodeReviewerAI.Services // Or CodeReviewerAI.Models if you prefer
{
    // This is the object that holds the AI's answer
    public record ReviewResult(string MarkdownReview, int RiskScore = 0);
}