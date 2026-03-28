using CodeReviewerAI.Models;
using CodeReviewerAI.Services;
using CodeReviewerAI.Services.Gemini;
using CodeReviewerAI.Services.Github;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Octokit;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace CodeReviewerAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostApplicationBuilder();
            builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection("Gemini"));
            builder.Services.Configure<GithubOptions>(builder.Configuration.GetSection("Github"));


           // ReviewerService reviewer = new ReviewerService(new GeminiServices(),new GithubServices(),new PromptService());
           /* if (args.Length != 3)
            {
                Console.Error.WriteLine("Missing argument in application like owner , reposName and prNumber");
                Environment.Exit(1);
            }
                if (!int.TryParse(args[2],out int prNumberOutput))
                {
                    Console.Error.WriteLine("prNumber is not a valid integer");
                    Environment.Exit(1);
                }
                
                    string owner = args[0];
                    string reposName = args[1];
                    int prNumber = prNumberOutput;
           */
            string owner = "ritchyboy";
            string reposName = "SandBox_Test";
            int prNumber = 2;


           /* ReviewResult result = await reviewer.ReviewPullrequestAsync(owner, reposName, prNumber);

            Console.WriteLine(result.MarkdownReview);
            Console.WriteLine("Summary: " + result.Summary);
            Console.WriteLine("IsApproved: " + result.IsApproved);
            Console.WriteLine("RiskLevel: " + result.RiskLevel);
            Console.WriteLine("RiskScore: " + result.RiskScore);
            Console.ReadLine();
           */
        }
    }
}
