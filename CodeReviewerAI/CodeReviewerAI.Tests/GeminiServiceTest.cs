using CodeReviewerAI.Models;
using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using CodeReviewerAI.Test.Services; // Ensure this matches your namespace
using CodeReviewerAI.Tests.Integration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using CodeReviewerAI.Services.IStrategy;

namespace CodeReviewerAI.Test.Services
{
    public class GeminiServiceTests : BaseIntegrationTest
    {

        [Fact]
        public void Service_Can_Be_Constructed()
        {
            // Act
            var service = serviceProvider.GetRequiredService<IGeminiServices>();
            // Assert
            Assert.NotNull(service);
        }

        // WARNING: This test calls the REAL API.
        [Fact]
        public async Task AnalyzeDiff_Returns_Real_Response_From_Google()
        {
            // Arrange
            var service = serviceProvider.GetRequiredService<IGeminiServices>();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string clientPathTest = Path.Combine(basePath,"Test", "ChatServerMain.cs");
            if (!File.Exists(clientPathTest))
            {
                throw new FileNotFoundException("The test file was not found");
            }

            string testableCode = await File.ReadAllTextAsync(clientPathTest);

            var strategyProvider = serviceProvider.GetRequiredService<IStrategyLanguage>();
            var promptService = new PromptService(strategyProvider);


            string request = await promptService.promptManagerAsync(clientPathTest,testableCode);

            // Act
            var result = await service.AnalyzeCodeToReview(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBe(string.IsNullOrEmpty(result.MarkdownReview));
        }

        [Fact]
        public async Task GetReviewAsync_WhenValidDiff_ShouldReturnAiAnalysis()
        {
           
            var service = serviceProvider.GetRequiredService<IGeminiServices>();
            var fakeDiff = "diff --git a/file.txt b/file.txt\n+ Console.WriteLine(\"Hello World\");";

            
            var result = await service.AnalyzeCodeToReview(fakeDiff);

          
            result.Should().NotBeNull();   
        }
    }
}
