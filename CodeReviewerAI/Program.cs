using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using System.Runtime.CompilerServices;

namespace CodeReviewerAI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Uri url = new Uri("https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent");
            var host = Host.CreateDefaultBuilder(args).ConfigureServices(services => {
                services.AddHttpClient<IGeminiServices,GeminiServices>(client => {
                    client.BaseAddress = url;
                });
            })
    .Build();
            /*
            string GEMINI_API_KEY = Environment.GetEnvironmentVariable("GEMINI_API_KEY").ToString();*/
        }

    }
}
