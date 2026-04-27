using CodeReviewerAI.Models;
using CodeReviewerAI.Services.Gemini;
using CodeReviewerAI.Services.IServices;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Resilience;
using Polly;
using Polly.Registry;
using System;
using System.Text.Json;
using System.Threading.Tasks;


public class GeminiServices: IGeminiServices
{
    
	private readonly Client _client;
    private readonly GeminiOptions _options;
    

    public GeminiServices(IOptions<GeminiOptions> options){ 
        _options = options.Value;
        ArgumentException.ThrowIfNullOrEmpty(_options.ApiKey, nameof(_options.ApiKey));
        ArgumentException.ThrowIfNullOrEmpty(_options.Model, nameof(_options.Model));
        _client = new Client(apiKey:_options.ApiKey);
    }

    public async Task<ReviewResult> AnalyzeCodeToReview(string request)
	{
        var response = await _client.Models.GenerateContentAsync(
        model: _options.Model, contents: request
    );
		string rawResponse = response.Candidates[0].Content.Parts[0].Text;
		if (string.IsNullOrEmpty(rawResponse))
		{
            Console.WriteLine("Gemini did not respond at all");
		}

        string cleanJson = rawResponse.Replace("```json", "").Replace("```", "").Trim();

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
        try
        {
            ReviewResult result = JsonSerializer.Deserialize<ReviewResult>(cleanJson, options);
            return result;
        }
        catch (JsonException ex)
        {
            return new ReviewResult
            {
                IsApproved = false,
                RiskLevel = "Critical",
                Summary = "AI returned invalid JSON.",
                MarkdownReview = $"### Parsing Error\nThe AI returned:\n{rawResponse}"
            };
        }
    }

}
