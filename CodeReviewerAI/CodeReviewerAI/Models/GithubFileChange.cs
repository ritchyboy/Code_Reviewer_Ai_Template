namespace CodeReviewerAI.Models
{
    public class GithubFileChange
    {
        public string fileHeader { get; set; }
        public string fileName { get; set; }
        public string patch { get; set; }
    }
}
