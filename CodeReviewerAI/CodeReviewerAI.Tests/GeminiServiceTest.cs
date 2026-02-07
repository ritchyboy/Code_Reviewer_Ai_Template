using Xunit;
using CodeReviewerAI.Test.Services; // Ensure this matches your namespace
using System;
using System.Threading.Tasks;
using CodeReviewerAI.Services;
using System.IO;

namespace CodeReviewerAI.Test.Services
{
    public class GeminiServiceTests
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


            var promptService = new PromptService();
            string generalPrompt = await promptService.promptManager(clientPathTest);
            

            string testableCode = await File.ReadAllTextAsync(clientPathTest);
            string promptWithCode = generalPrompt + "\n" + testableCode;

            // Act
            var result = await service.AnalyzeCodeToReview(testableCode);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.MarkdownReview), "The AI returned an empty string!");

        }
    }
}
