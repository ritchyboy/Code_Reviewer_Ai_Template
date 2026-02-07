using CodeReviewerAI.Models;
using CodeReviewerAI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Tests
{
    public class ReviewerServiceTest
    {
        public string GetApiKey()
        {
            string key = Environment.GetEnvironmentVariable("GEMINI_API_KEY"
            , EnvironmentVariableTarget.User);

            return key;
        }
        public string GetToken()
        {
            string token = Environment.GetEnvironmentVariable("Github_Token"
                , EnvironmentVariableTarget.User);

            return token;
        }
        [Fact]
        public void can_the_service_be_build()
        {
            var geminiService = new GeminiServices(GetApiKey());
            var githubService = new GithubServices(GetToken());

            // ACT
            
           var service = new ReviewerService(geminiService, githubService);

            Assert.NotNull(service);
        }
        [Fact]
        public async Task pullrequest_review_result_verification()
        {
            //RepoTest
            string owner = "ritchyboy";
            string reposName = "SandBox_Test";
            int prNumber = 1;

            // Service
            var geminiService = new GeminiServices(GetApiKey());
            var githubService = new GithubServices(GetToken());
            var ReviewerService = new ReviewerService(geminiService,githubService);


            ReviewResult result = await ReviewerService.ReviewPullrequestAsync(owner,reposName,prNumber);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.MarkdownReview),"Models didn't return a comment");
        }
    }
}
