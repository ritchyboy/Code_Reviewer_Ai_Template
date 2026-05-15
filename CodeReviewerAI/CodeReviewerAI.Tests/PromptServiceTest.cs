using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IStrategy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewerAI.Tests.Integration
{
    public class PromptServiceTest : BaseIntegrationTest
    {
        [Fact]
        public void can_be_constructed()
        {
            var strategyProvider = serviceProvider.GetRequiredService<IStrategyLanguage>();

            var service = new PromptService(strategyProvider);

            service.Should().NotBeNull();
        }

        [Fact]
        public async Task prompt_is_build()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string clientPathTest = Path.Combine(basePath, "Test", "ChatServerMain.cs");
            if (!File.Exists(clientPathTest))
            {
                throw new FileNotFoundException("The test file was not found");
            }

            string testableCode = await File.ReadAllTextAsync(clientPathTest);

            var strategyProvider = serviceProvider.GetRequiredService<IStrategyLanguage>();
            var service = new PromptService(strategyProvider);

            string result = await service.promptManagerAsync(clientPathTest, testableCode);

            result.Should().NotBeNullOrEmpty("The prompt cannot be build correctly");
        }
    }
}
