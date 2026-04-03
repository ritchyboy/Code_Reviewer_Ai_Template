using CodeReviewerAI.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Tests
{
    public class PromptServiceTest
    {
        [Fact]
        public void can_be_constructed()
        {
            var service = new PromptService();

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
            var service = new PromptService();
            string result = await service.promptManagerAsync(clientPathTest, testableCode);

            result.Should().NotBeNullOrEmpty("The prompt cannot be build correctly");
        }
    }
}
