using CodeReviewerAI.Services;
using CodeReviewerAI.Services.Gemini;
using CodeReviewerAI.Services.Github;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Tests.Integration
{
    public abstract class BaseIntegrationTest
    {
        protected readonly IServiceProvider serviceProvider;

        protected BaseIntegrationTest()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true).AddUserSecrets<BaseIntegrationTest>()
            .Build();

            var service = new ServiceCollection();

            service.Configure<GeminiOptions>(configuration.GetSection("Gemini"));
            service.Configure<GithubOptions>(configuration.GetSection("Github"));

            service.AddScoped<IGeminiServices, GeminiServices>();
            service.AddScoped<IGithubServices, GithubServices>();

            serviceProvider = service.BuildServiceProvider();
        }

    }
}
