namespace CodeReviewerAI.Data.Models
{
    public class AiModelsReviewer
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public string Provider { get; set; }
        public double Temperature { get; set; }
    }
}
