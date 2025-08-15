using CodeReviewerAI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CodeReviewerAI.Services
{
    public class Content
    {
        public Part[] parts; 
    }

    public class Part
    {
        public string? text { get; set; }
    }

    public class GeminiGenerateText
    {
        public Content[] contents;
    }

    public class GeminiServices : IGeminiServices
    {
        public string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
        public string geminiMessage = "say hola and nothing else,this is a test";

        private readonly IHttpClientFactory _httpClientFactory;

        public GeminiServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task GetFileReviewAsync()
        {
            var ClientToGemini = _httpClientFactory.CreateClient();

            var geminiRequest = new GeminiGenerateText
            {
                contents = [new Content { parts = [new Part { text = geminiMessage }] }]
            };

            var response = await ClientToGemini.PostAsJsonAsync(url, geminiRequest);
            if (response.IsSuccessStatusCode)
            {
                string? value = response.Content.ReadAsStreamAsync().ToString();
                Console.WriteLine(value);
            }
            else
            {
                Console.WriteLine("Error: Your request could not be post");
            }
        }
            
        
    }
}
