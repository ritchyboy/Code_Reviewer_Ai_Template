using CodeReviewerAI.Services;
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

            Assert.NotNull(service);
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


            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result), "The prompt cannot be build correctly");
        }
    }
}
