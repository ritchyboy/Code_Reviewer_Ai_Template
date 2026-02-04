using Xunit;
using CodeReviewerAI.Test.Services; // Ensure this matches your namespace
using System;
using System.Threading.Tasks;

namespace CodeReviewerAI.Test.Services
{
    public class GeminiServiceTests
    {
        private string GetApiKey()
        {
            var key = Environment.GetEnvironmentVariable("GEMINI_API_KEY", EnvironmentVariableTarget.User);

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
            var service = new GeminiServices2(key); // Assuming your cleaned-up class looks like this

            // Assert
            Assert.NotNull(service);
        }

        // WARNING: This test calls the REAL API.
        [Fact]
        public async Task AnalyzeDiff_Returns_Real_Response_From_Google()
        {
            // Arrange
            var service = new GeminiServices2(GetApiKey());

            string path = "C:\\Users\\Ritch\\source\\nullashrepos\\CodeReviewerAI\\CodeReviewerAI.Tests\\Test\\ChatClientMain.cs";
            string fileRead = await File.ReadAllTextAsync(path);

            // Act
            var result = await service.AnalyzeCodeToReview(fileRead);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.MarkdownReview), "The AI returned an empty string!");

        }
    }
}
