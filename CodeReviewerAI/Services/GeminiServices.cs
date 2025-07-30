using CodeReviewerAI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services
{
    public class GeminiServices : IGeminiServices
    {
        public string url = "";

        private readonly IHttpClientFactory _httpClientFactory;

        public GeminiServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task GetFileReviewAsync(string fileName)
        {
            
        }
    }
}
