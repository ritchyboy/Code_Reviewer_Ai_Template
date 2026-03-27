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

namespace CodeReviewerAI.Test.Services
{
    public class GeminiServiceTests : BaseIntegrationTest
    {
        private string GetApiKey()
        {
            var key = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            if(string.IsNullOrEmpty(key))
               key = Environment.GetEnvironmentVariable("GEMINI_API_KEY", EnvironmentVariableTarget.User);
              

            // Helpful error if you forgot to restart Visual Studio
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("API Key not found! Did you restart Visual Studio/Terminal after setting 'GEMINI_API_KEY'?");

            return key;
        }

        [Fact]
        public void Service_Can_Be_Constructed()
        {
            // Arrange
            string key = GetApiKey();

            // Act
            var service = new GeminiServices(key); // Assuming your cleaned-up class looks like this

            // Assert
            Assert.NotNull(service);
        }

        // WARNING: This test calls the REAL API.
        [Fact]
        public async Task AnalyzeDiff_Returns_Real_Response_From_Google()
        {
            // Arrange
            var service = new GeminiServices(GetApiKey());

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string clientPathTest = Path.Combine(basePath,"Test", "ChatServerMain.cs");
            if (!File.Exists(clientPathTest))
            {
                throw new FileNotFoundException("The test file was not found");
            }

            string testableCode = await File.ReadAllTextAsync(clientPathTest);

            var promptService = new PromptService();
            string generalPrompt = await promptService.promptManagerAsync(clientPathTest,testableCode);

            // Act
            var result = await service.AnalyzeCodeToReview(testableCode);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.MarkdownReview), "The AI returned an empty string!");

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
