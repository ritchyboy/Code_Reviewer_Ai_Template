using System.ComponentModel.DataAnnotations;


namespace CodeReviewerAI.Data.Models
{
    public sealed class ReviewRecord
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string RepoName { get; set; } 
        public int PrNumber { get; set; }
        public string RiskLevel { get; set; }
        public double RiskScore { get; set; }
        public double ExecutionTime { get; set; }
        public DateTime DateReviewed { get; set; } = DateTime.UtcNow;


        public int ModelId { get; set; }
        public AiModelsReviewer Model { get; set; }


        public List<FileReview> Files { get; set; } = new();
    }
}
