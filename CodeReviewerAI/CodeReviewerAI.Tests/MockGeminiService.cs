using CodeReviewerAI.Services.Gemini;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Tests
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
