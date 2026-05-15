using CodeReviewerAI.Services.Gemini;
using Microsoft.Extensions.Options;

namespace CodeReviewerAI.Tests.Unit
{
    public class MockGeminiService
    {
        [Fact]
        public void initiate_mock_gemini_option_service()
        {
            var mockOption = Options.Create(new GeminiOptions
            {
                ApiKey = "Alfrediskey",
                Model = "gemini-3-flash-preview",
                Provider = "Google"
            });

            var service = new GeminiServices(mockOption);

            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WhenApiKeyIsMissing_ShouldThrowArgumentException()
        {
            
            var options = Options.Create(new GeminiOptions { ApiKey = "" });

            Assert.Throws<ArgumentException>(() => new GeminiServices(options));
        }
        
        
    }
}
