using CodeReviewerAI.Models;
using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using CodeReviewerAI.Tests.Integration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Tests
{
    public class ReviewerServiceTest : BaseIntegrationTest
    {

        [Fact]
        public void can_the_service_be_build()
        {
            // ACT
            var geminiService = serviceProvider.GetRequiredService<IGeminiServices>();
            var githubService = serviceProvider.GetRequiredService<IGithubServices>();
            var promptService = serviceProvider.GetRequiredService<IPromptService>();
            var service = new ReviewerService(geminiService, githubService, promptService);

            service.Should().NotBeNull();
        }
        [Fact]
        public async Task pullrequest_review_result_verification()
        {
            //RepoTest
            string owner = "ritchyboy";
            string reposName = "SandBox_Test";
            int prNumber = 2;

            // Service
            var geminiService = serviceProvider.GetRequiredService<IGeminiServices>();
            var githubService = serviceProvider.GetRequiredService<IGithubServices>();
            var promptService = serviceProvider.GetRequiredService<IPromptService>();
            var service = new ReviewerService(geminiService, githubService, promptService);


            ReviewResult result = await service.ReviewPullrequestAsync(owner, reposName, prNumber);

            result.Should().NotBeNull();
            result.MarkdownReview.Should().NotBeNullOrEmpty("Models didn't return a comment");
        }
        
    }
}
