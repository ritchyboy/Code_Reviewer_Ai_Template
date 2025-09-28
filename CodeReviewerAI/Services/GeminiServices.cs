using CodeReviewerAI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace CodeReviewerAI.Services
{

    public class GeminiServices : IGeminiServices
    {
        
        private string GEMINI_API_KEY;
        public string valueInString;
        public string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
        public static string filePath = @"C:\Users\Shiin\source\temp\CodeReviewerAI\CodeReviewerAI\Config\Prompt.txt";
        string fileContent = File.ReadAllText(filePath);    

        public string InitPrompt(string message)
        {
             string tempFileContent = fileContent.Replace("CODE_CONTENT", message);
             string requestString = @"{
             ""contents"": [
             {
                   ""parts"": [
                   {
                        ""text"":  """ + tempFileContent + @"""
             }
                        ]
                        }
             ]
            
             }";
            return requestString;
        }

        

        public async Task<string> GetReviewFromCode(string codeToReview)
        {
            string requestString = InitPrompt(codeToReview);    
            StringContent content = new StringContent(requestString); 
            using (HttpClient client = new HttpClient())
            {
                if (string.IsNullOrEmpty(GEMINI_API_KEY))
                {
                    Console.WriteLine("GEMINI API KEY is empty or null");
                    client.Dispose();
                }
                else
                   client.DefaultRequestHeaders.Add("x-goog-api-key", GEMINI_API_KEY);

                try
                {
                    var result = await client.PostAsync(url, content);
                    if (result.IsSuccessStatusCode)
                    {
                       valueInString = await result.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {result.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            string? promptResult;
            using (JsonDocument document = JsonDocument.Parse(valueInString))
            {
                JsonElement candidates = document.RootElement.GetProperty("candidates");
                JsonElement firstCandidates = candidates[0];
                JsonElement element = firstCandidates.GetProperty("content");
                JsonElement firstElement = element;
                JsonElement parts = firstElement.GetProperty("parts");
                JsonElement firstParts = parts[0];
                JsonElement textElement = firstParts.GetProperty("text");
                promptResult = textElement.GetString();
            }
            return promptResult;
        }
        public string SET_API_KEY(string geminiApikey)
        {
            return GEMINI_API_KEY = geminiApikey;
        }
            
        
    }
}
