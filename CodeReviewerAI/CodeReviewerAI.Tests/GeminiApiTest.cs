using Xunit;
using CodeReviewerAI.Test.Services; // Ensure this matches your namespace
using System;
using System.Threading.Tasks;

namespace CodeReviewerAI.Test.Services
{
    public class GeminiServiceTests
    {
        // HELPER: Get the key safely from your User Environment Variables
        private string GetApiKey()
        {
            var key = Environment.GetEnvironmentVariable("GEMINI_API_KEY", EnvironmentVariableTarget.User);

            // Helpful error if you forgot to restart Visual Studio
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("❌ API Key not found! Did you restart Visual Studio/Terminal after setting 'GEMINI_API_KEY'?");

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

        // ⚠️ WARNING: This test calls the REAL API.
        // We use [Fact] so it runs. If you want to skip it later, use [Fact(Skip = "Cost money")]
        [Fact]
        public async Task AnalyzeDiff_Returns_Real_Response_From_Google()
        {
            // Arrange
            var service = new GeminiServices2(GetApiKey());
            string fakeDiff = "public void Test() { int x = \"Bad Type\"; }"; // Broken code on purpose

            // Act
            // Note: We expect your method to be named 'AnalyzeDiffAsync' based on our clean architecture
            var result = await service.AnalyzeCodeToReview(fakeDiff);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.MarkdownReview), "The AI returned an empty string!");

            // Optional: Print it to the Test Output (Debug console) to see what Gemini thinks
            // (You can see this in Visual Studio Test Explorer -> Output)
        }
    }
}
