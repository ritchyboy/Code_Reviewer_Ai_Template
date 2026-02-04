using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Octokit;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace CodeReviewerAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string testableFile = Path.Combine(basePath,"Test","ChatServerMain.cs");
            if (!File.Exists(testableFile))
            {
                throw new FileNotFoundException("The file is in the folder Test was not found");
            }
            string readTestableFile = File.ReadAllText(testableFile);
            FileInfo testInfo = new FileInfo(testableFile);

            var prompt = new PromptService();
            string fullPromptMessage = prompt.promptManager(testInfo) + "\n" + readTestableFile;

            Console.WriteLine(fullPromptMessage);
            Console.ReadLine();
        }

    }
}
