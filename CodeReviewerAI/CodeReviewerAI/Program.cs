using CodeReviewerAI.Models;
using CodeReviewerAI.Services;
using CodeReviewerAI.Services.Gemini;
using CodeReviewerAI.Services.Github;
using CodeReviewerAI.Services.IServices;
using CodeReviewerAI.Services.IStrategy;
using CodeReviewerAI.Services.Strategy;
using Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Registry;
using Polly.Retry;
using System;

namespace CodeReviewerAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR:Missing argument,Incorrect number of arguments.");
                Console.ResetColor();
                Console.WriteLine("Usage: CodeReviewerAI <owner> <repo> <prNumber>");
                Console.WriteLine("Example: dotnet run -- Alfred Sandbox_Project 5");
                Environment.Exit(1);
            }
            if (!int.TryParse(args[2], out int prNumberOutput))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: prNumber is not a valid integer");
                Console.ResetColor();
                Environment.Exit(1);
            }
            string owner = args[0];
            string reposName = args[1];
            int prNumber = prNumberOutput;


            var builder = new HostApplicationBuilder();
            builder.Configuration.AddJsonFile("appsettings.json", true, true)
            .AddUserSecrets<Program>(optional:true);

            builder.Services.AddResiliencePipeline("Default", x =>
            {
                x.AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<GoogleApiException>(),
                    Delay = TimeSpan.FromSeconds(4),
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true
                });
            });

            builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection("Gemini"));
            builder.Services.Configure<GithubOptions>(builder.Configuration.GetSection("Github"));

            builder.Services.Scan(scan => scan.FromAssemblyOf<ILanguageStrategy>()
            .AddClasses(classes => classes.AssignableTo<ILanguageStrategy>()).AsImplementedInterfaces()
            .WithScopedLifetime());

            builder.Services.AddScoped<LanguageStrategyProvider>();

            builder.Services.AddScoped<GeminiService>();
            builder.Services.AddScoped<IGeminiServices>(ServiceProvider =>
            {
                var coreService = ServiceProvider.GetRequiredService<GeminiService>();

                var pipelineProvider = ServiceProvider.GetRequiredService<ResiliencePipelineProvider<string>>();

                return new ResilientGeminiServices(coreService, pipelineProvider);
            });
            builder.Services.AddScoped<IGithubServices, GithubService>();
            builder.Services.AddScoped<ILanguageStrategyProvider,LanguageStrategyProvider>();
            builder.Services.AddScoped<IPromptService, PromptService>();
            builder.Services.AddScoped<IReviewerService, ReviewerService>();

            using (IHost host = builder.Build())
            {
                using (IServiceScope serviceScope = host.Services.CreateScope())
                {
                    IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                    var service = serviceProvider.GetRequiredService<IReviewerService>();
                    var githubService = serviceProvider.GetRequiredService<IGithubServices>();


                    var result = await service.ReviewPullrequestAsync(owner,reposName,prNumber);
                    await githubService.CreateReviewCommentAsync(owner, reposName,prNumber,result);
                }
            }
        }
    }
}
