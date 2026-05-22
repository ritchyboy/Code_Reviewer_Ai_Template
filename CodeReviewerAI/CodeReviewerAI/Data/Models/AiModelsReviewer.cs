namespace CodeReviewerAI.Data.Models
{
    public sealed class AiModelsReviewer
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Provider { get; set; }
        public double Temperature { get; set; }
    }
}
