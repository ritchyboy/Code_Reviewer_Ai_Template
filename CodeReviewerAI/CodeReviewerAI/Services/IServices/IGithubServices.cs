using CodeReviewerAI.Models;

namespace CodeReviewerAI.Services.IServices
{
    public interface IGithubServices
    {
        public Task<List<GithubFileChange>> GetPullRequestDiffsAsync(string owner,string repoName,int prNumber);
        public Task CreateReviewCommentAsync(string owner, string repoName, int prNumber,ReviewResult pullRequestComment);
    }
}
