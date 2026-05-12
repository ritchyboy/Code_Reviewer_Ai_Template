using CodeReviewerAI.Models;

namespace CodeReviewerAI.Services.IServices
{
    public interface IGithubServices
    {
        public Task<List<GithubFileChange>> pullRequestDiffs(string owner,string repoName,int prNumber);
        public Task createReviewCommentAsync(string owner, string repoName, int prNumber,ReviewResult pullRequestComment);
    }
}
