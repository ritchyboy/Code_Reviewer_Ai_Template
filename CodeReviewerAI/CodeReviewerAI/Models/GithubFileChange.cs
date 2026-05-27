namespace CodeReviewerAI.Models
{
    public sealed class GithubFileChange
    {
        public string FileHeader { get; set; }
        public string FileName { get; set; }
        public string Patch { get; set; }
    }
}
