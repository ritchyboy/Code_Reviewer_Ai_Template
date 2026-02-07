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
        static async Task Main(string[] args)
        {

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
        private static string GetApiKey()
        {
            var key = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            if (string.IsNullOrEmpty(key))
                key = Environment.GetEnvironmentVariable("GEMINI_API_KEY", EnvironmentVariableTarget.User);

            // Helpful error if you forgot to restart Visual Studio
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("API Key not found! Did you restart Visual Studio/Terminal after setting 'GEMINI_API_KEY'?");

            return key;
        }
        private static string GetToken()
        {
            string token = Environment.GetEnvironmentVariable("Github_Token");

            if(string.IsNullOrEmpty(token))
                token = Environment.GetEnvironmentVariable("Github_Token",
                EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException("Token was not found ! You should try to restart your IDE after setting GITHUB_TOKEN");

            return token;
        }

    }
}
