using CodeReviewerAI.Models;
namespace CodeReviewerAI.Services.IServices
{
    public interface IReviewerService
    {
        public Task<ReviewResult> ReviewPullrequestAsync(string owner, string reposName, int prNumber);
        public Task<ReviewResult> RunAsync(string owner, string reposName, int prNumber);
    }
}
