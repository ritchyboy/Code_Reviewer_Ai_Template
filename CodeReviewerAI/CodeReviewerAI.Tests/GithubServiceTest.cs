using CodeReviewerAI.Models;
using CodeReviewerAI.Services;

namespace CodeReviewerAI.Tests.Services
{
    public class GithubServiceTest
    {
        private string GetGithubToken()
        {
            string token = Environment.GetEnvironmentVariable("GIT_TOKEN");

            if (string.IsNullOrEmpty(token))
                token = Environment.GetEnvironmentVariable("GIT_TOKEN",
                EnvironmentVariableTarget.User);

            return token;
        }
        [Fact]
        public void services_can_be_constructed()
        {
            // Arrange
            string githubToken = GetGithubToken();

            // Act
            var services = new GithubServices(githubToken);

            // Assert
            Assert.NotNull(services);
        }

        [Fact]
        public void get_data_from_service()
        {
            var service = new GithubServices(GetGithubToken());

            service.getDataFromUser();
        }
        [Fact]
        public async Task Analyze_PullRequest_Diff_For_Response_From_Github()
        {
            string owner = "ritchyboy";
            string reposName = "SandBox_Test";


            var service = new GithubServices(GetGithubToken());
            var diffs = new List<GithubFileChange>();
            diffs = await service.pullRequestDiffs(owner,reposName,1);

            
            Assert.True(diffs.Count > 0);
            Assert.Contains("Hello Github PullRequest", diffs[0].patch);
        }
    }
}
