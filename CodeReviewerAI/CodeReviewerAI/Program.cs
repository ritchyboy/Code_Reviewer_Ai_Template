using CodeReviewerAI.Models;
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
        private static string GetApiKey()
        {
            var key = Environment.GetEnvironmentVariable("GEMINI_API_KEY", EnvironmentVariableTarget.User);

            // Helpful error if you forgot to restart Visual Studio
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("API Key not found! Did you restart Visual Studio/Terminal after setting 'GEMINI_API_KEY'?");

            return key;
        }
        private static string GetToken()
        {
            string token = Environment.GetEnvironmentVariable("Github_Token",
                EnvironmentVariableTarget.User);

            return token;
        }
        static async Task Main(string[] args)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string testableFile = Path.Combine(basePath,"Test","ChatServerMain.cs");
            if (!File.Exists(testableFile))
            {
                throw new FileNotFoundException("The file is in the folder Test was not found");
            }
            string readTestableFile = File.ReadAllText(testableFile);

            var prompt = new PromptService();
            string fullPromptMessage = prompt.promptManager(testableFile) + "\n" + readTestableFile;

            /*    var service = new GeminiServices(GetApiKey());
                ReviewResult geminiResponse = await service.AnalyzeCodeToReview(fullPromptMessage);

                Console.WriteLine("IsApproved: " + geminiResponse.IsApproved);
                Console.WriteLine("RiskLevel: " + geminiResponse.RiskLevel);
                Console.WriteLine("RiskScore: " + geminiResponse.RiskScore);
                Console.WriteLine("Summary: " + geminiResponse.Summary);
                Console.WriteLine("MarkdownReview: " + geminiResponse.MarkdownReview);
                Console.ReadLine();
            
            string githubToken = Environment.GetEnvironmentVariable("Github_Token",
            EnvironmentVariableTarget.User);
            var githubService = new GithubServices(githubToken);
            string dataRetrieveTest = githubService.getDataFromUser();
            string currentRequestInfo = githubService.getApiInfo();
            Console.WriteLine(dataRetrieveTest);
            Console.WriteLine(currentRequestInfo);
            Console.ReadLine();
            */

            var geminiService = new GeminiServices(GetApiKey());
            var githubService = new GithubServices(GetToken());
            ReviewerService reviewer = new ReviewerService(geminiService,githubService);


            string owner = "ritchyboy";
            string reposName = "SandBox_Test";
            int prNumber = 1;


            ReviewResult result = await reviewer.ReviewPullrequestAsync(owner, reposName, prNumber);

            Console.WriteLine(result.MarkdownReview);
            Console.ReadLine();
        }

    }
}
