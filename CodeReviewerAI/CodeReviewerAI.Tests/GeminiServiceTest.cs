using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using CodeReviewerAI.Services.IStrategy;

namespace CodeReviewerAI.Tests.Integration
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

            var strategyProvider = serviceProvider.GetRequiredService<ILanguageStrategyProvider>();
            var promptService = new PromptService(strategyProvider);


            string request = await promptService.GetCompletePromptAsync(clientPathTest,testableCode);

            // Act
            var result = await service.AnalyzeCodeToReviewAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBe(string.IsNullOrEmpty(result.MarkdownReview));
        }

        [Fact]
        public async Task GetReviewAsync_WhenValidDiff_ShouldReturnAiAnalysis()
        {
           
            var service = serviceProvider.GetRequiredService<IGeminiServices>();
            var fakeDiff = "diff --git a/file.txt b/file.txt\n+ Console.WriteLine(\"Hello World\");";

            
            var result = await service.AnalyzeCodeToReviewAsync(fakeDiff);

          
            result.Should().NotBeNull();   
        }
    }
}
