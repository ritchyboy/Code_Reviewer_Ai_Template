using CodeReviewerAI.Services.IServices;
using Octokit;

namespace CodeReviewerAI.Services
{
    public class GithubServices: IGithubServices
    {
        private readonly GitHubClient _client;
        public string _appName = "CodeReviewerAI";
        public GithubServices(string appName)
        {
            _appName = appName;
            _client = new GitHubClient(new ProductHeaderValue(appName));
        }

        public Task<List<string>> pullRequestDiffs(string owner, string repoName, int prNumber)
        {
            throw new NotImplementedException();
        }
    }
}
