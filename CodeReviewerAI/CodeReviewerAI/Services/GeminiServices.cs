using Azure;
using CodeReviewerAI.Models;
using CodeReviewerAI.Services.Gemini;
using CodeReviewerAI.Services.IServices;
using Google.GenAI;
using Microsoft.Extensions.Options;
using Octokit;
using System.Text.Json;
using System.Threading.RateLimiting;


public class GeminiServices : IGeminiServices
{

    private readonly Client _client;
    private readonly GeminiOptions _options;
    private readonly RateLimiter _rateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
    {
        PermitLimit = 15,
        Window = TimeSpan.FromMinutes(1),
        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        QueueLimit = 100
    });
    

    public GeminiServices(IOptions<GeminiOptions> options){ 
        _options = options.Value;
        ArgumentException.ThrowIfNullOrEmpty(_options.ApiKey, nameof(_options.ApiKey));
        ArgumentException.ThrowIfNullOrEmpty(_options.Model, nameof(_options.Model));
        _client = new Client(apiKey:_options.ApiKey);
    }

    public async Task<ReviewResult> AnalyzeCodeToReview(string request)
	{
        using var lease = await _rateLimiter.AcquireAsync(permitCount: 1);
        if (lease.IsAcquired)
        {

            var response = await _client.Models.GenerateContentAsync(
            model: _options.Model, contents: request);

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
        else
        {
            throw new RateLimitExceededException((IResponse)_rateLimiter.GetStatistics());
        }
    }

}
