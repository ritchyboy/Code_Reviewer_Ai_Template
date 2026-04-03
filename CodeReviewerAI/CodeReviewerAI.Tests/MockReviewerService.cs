using CodeReviewerAI.Models;
using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Apis.Util;

namespace CodeReviewerAI.Tests
{
    public class MockReviewerService
    {
        [Fact]
        public async Task ReviewerService_Should_Return_Analysis()
        {
            var mockGemini = new Mock<IGeminiServices>();
            var mockGithub = new Mock<IGithubServices>();
            var mockPrompt = new Mock<IPromptService>();

            string mockOwner = "Alfred";
            string mockReposName = "Sandbox_Project";
            int mockPrNumber = 5;

            mockGemini.Setup(x => x.AnalyzeCodeToReview(It.IsAny<string>()))
                .ReturnsAsync(new ReviewResult
                {
                    IsApproved = false,
                    RiskLevel = "High Severity",
                    RiskScore = 10,
                    Summary = "Bad code",
                    MarkdownReview = "Mocked: This code has a security flaw."
                });
            mockGithub.Setup(x => x.pullRequestDiffs(mockOwner, mockReposName, mockPrNumber))
                .ReturnsAsync(new List<GithubFileChange>(){
                    new GithubFileChange
                    {
                       fileName = "Sandbox_Test.cs",
                       fileHeader = $"---FILE: {"Sandbox_Test.cs"}---\n",
                       patch = "Console.WriteLine(\"Hello World\""
                    }
                });

            mockPrompt.Setup(x => x.promptManagerAsync(It.IsAny<string>(),It.IsAny<string>()))
               .ReturnsAsync(It.IsAny<string>());

            var sut = new ReviewerService(mockGemini.Object, mockGithub.Object, mockPrompt.Object);
            var result = await sut.ReviewPullrequestAsync(mockOwner, mockReposName, mockPrNumber);

            result.Should().NotBeNull();
            result.MarkdownReview.Should().Be("Mocked: This code has a security flaw.");

            mockGemini.Verify(x => x.AnalyzeCodeToReview(It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public async Task ReviewerService_Should_Return_An_Error_When_Prompt_Is_Empty()
        {
            var mockGemini = new Mock<IGeminiServices>();
            var mockGithub = new Mock<IGithubServices>();
            var mockPrompt = new Mock<IPromptService>();

            string mockOwner = "Alfred";
            string mockReposName = "Sandbox_Project";
            int mockPrNumber = 5;


            mockPrompt.Setup(x => x.promptManagerAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(string.Empty);

            var sut = new ReviewerService(mockGemini.Object, mockGithub.Object, mockPrompt.Object);

            Func<Task> result = async () => await sut.ReviewPullrequestAsync(mockOwner, mockReposName, mockPrNumber);
            await result.Should().ThrowAsync<NullReferenceException>();

            mockGemini.Verify(x => x.AnalyzeCodeToReview(It.IsAny<string>()), Times.Never);
        }
    }
}
