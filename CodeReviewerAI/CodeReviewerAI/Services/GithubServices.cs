using CodeReviewerAI.Models;
using CodeReviewerAI.Services.IServices;
using Google.GenAI;
using Octokit;

namespace CodeReviewerAI.Services
{
    public class GithubServices: IGithubServices
    {
        private readonly GitHubClient _client;
        public string _appName = "CodeReviewerAI";
        public GithubServices(string token)
        {
            _client = new GitHubClient(new ProductHeaderValue(_appName));
            
            var tokenAuth = new Credentials(token);
            _client.Credentials = tokenAuth;
        }

        public async Task<List<GithubFileChange>> pullRequestDiffs(string owner, string repoName, int prNumber)
        {
            var pullRequestFiles = await _client.Repository.PullRequest.Files(owner, repoName, prNumber);
            var diffs = new List<GithubFileChange>();
            
            foreach(var file in pullRequestFiles)
            {
                if (file.Status == "removed" || IsBinary(file.FileName)) continue;

                string fileHeader = $"Code from: {file.FileName} \n";
                if (string.IsNullOrEmpty(file.Patch))
                {
                    Console.WriteLine("No change has been detected");
                }
                string filePatch = file.Patch;
                var fileInfoData = new GithubFileChange()
                {
                    fileName = fileHeader,
                    patch = filePatch
                };
                diffs.Add(fileInfoData);
            }

            return diffs;
        }
        private bool IsBinary(string filename)
        {
            string[] binaryExtensions = { ".png", ".jpg", ".jpeg", ".dll", ".exe", ".pdb" };
            return binaryExtensions.Any(ext => filename.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
        public string getDataFromUser()
        {
            var user = _client.User.Get("ritchyboy");
            return user.Result.OwnedPrivateRepos.ToString();
        }
        public string getApiInfo()
        {
            var apiInfo = _client.GetLastApiInfo();

            // If the ApiInfo isn't null, there will be a property called RateLimit
            var rateLimit = apiInfo?.RateLimit;

            var howManyRequestsCanIMakePerHour = rateLimit?.Limit;
            var howManyRequestsDoIHaveLeft = rateLimit?.Remaining;
            var whenDoesTheLimitReset = rateLimit?.Reset; // UTC time

            string fullInfo = "RateLimit" + "\n" +
                "RequestLimitPerHour: " + howManyRequestsCanIMakePerHour + "\n" +
                "RequestLeft: " + howManyRequestsDoIHaveLeft + "\n" +
                "ResetTimer: " + whenDoesTheLimitReset;

            return fullInfo;

        }
    }
}
