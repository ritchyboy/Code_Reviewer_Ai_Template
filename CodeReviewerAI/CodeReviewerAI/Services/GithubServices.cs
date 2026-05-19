using CodeReviewerAI.Models;
using CodeReviewerAI.Services.Github;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.Options;
using Octokit;
using System.Text;

namespace CodeReviewerAI.Services
{
    public class GithubServices: IGithubServices
    {
        private readonly GitHubClient _client;
        private readonly GithubOptions _options;

        public GithubServices(IOptions<GithubOptions> options)
        {
            _options = options.Value;
            _client = new GitHubClient(new ProductHeaderValue(_options.AppName));
            _client.Credentials = new Credentials(_options.Token);
            ArgumentException.ThrowIfNullOrEmpty(_options.AppName,nameof(_options.AppName));
            ArgumentException.ThrowIfNullOrEmpty(_options.Token,nameof(_options.Token));
        }

        public async Task<List<GithubFileChange>> pullRequestDiffs(string owner, string repoName, int prNumber)
        {
            var pullRequestFiles = await _client.Repository.PullRequest.Files(owner, repoName, prNumber);
            var diffs = new List<GithubFileChange>();
            
            foreach(var file in pullRequestFiles)
            {
                if (file.Status == "removed" || IsBinary(file.FileName)) continue;

                string fileHeader = $"---FILE: {file.FileName}---\n";
                if (string.IsNullOrEmpty(file.Patch))
                {
                    Console.WriteLine($"{file.FileName}: No change has been detected");
                }
                string filePatch = file.Patch;
                var fileInfoData = new GithubFileChange()
                {
                    fileHeader = fileHeader,
                    fileName = file.FileName,
                    patch = filePatch
                };
                diffs.Add(fileInfoData);
            }

            return diffs;
        }
        public async Task createReviewCommentAsync(string owner, string repoName, int prNumber,ReviewResult pullRequestComment)
        {
            StringBuilder commentBuilder = new StringBuilder();
            commentBuilder.AppendLine($"# CodeReviewerAI Report");
            commentBuilder.AppendLine($"**Status:** {(pullRequestComment.IsApproved ? "Approved" : "Changes Requested")}");
            commentBuilder.AppendLine($"**Risk Level:** {pullRequestComment.RiskLevel} ({pullRequestComment.RiskScore}/10)");
            commentBuilder.AppendLine("-----------------------------------------------");
            commentBuilder.AppendLine("--Summary--");
            commentBuilder.AppendLine(pullRequestComment.Summary);
            commentBuilder.AppendLine("--Detailed Review--");
            commentBuilder.AppendLine(pullRequestComment.MarkdownReview);
            await _client.Issue.Comment.Create(owner, repoName, prNumber,commentBuilder.ToString());
        }
        private bool IsBinary(string filename)
        {
            string[] binaryExtensions = { ".png", ".jpg", ".jpeg", ".dll", ".exe", ".pdb",".yml" };
            return binaryExtensions.Any(ext => filename.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
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
