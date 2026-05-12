using System.ComponentModel.DataAnnotations;


namespace CodeReviewerAI.Services.Github
{
    public class GithubOptions
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        public string AppName { get; set; } = string.Empty;
    }
}
