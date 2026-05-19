namespace CodeReviewerAI.Models
{
    public class GithubFileChange
    {
        public string FileHeader { get; set; }
        public string FileName { get; set; }
        public string patch { get; set; }
    }
}
