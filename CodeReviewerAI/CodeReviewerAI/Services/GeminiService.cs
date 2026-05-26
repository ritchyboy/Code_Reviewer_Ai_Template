using Azure;
using CodeReviewerAI.Models;
using CodeReviewerAI.Services.Gemini;
using CodeReviewerAI.Services.IServices;
using Google.GenAI;
using Microsoft.Extensions.Options;
using Octokit;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.RateLimiting;


public class GeminiService : IGeminiServices, IDisposable
{

    private readonly Client _client;
    private readonly GeminiOptions _options;
    private readonly RateLimiter _rateLimiter;
    public bool _disposed;
    

    public GeminiService(IOptions<GeminiOptions> options){ 
        _options = options.Value;
        ArgumentException.ThrowIfNullOrEmpty(_options.ApiKey, nameof(_options.ApiKey));
        ArgumentException.ThrowIfNullOrEmpty(_options.Model, nameof(_options.Model));
        _client = new Client(apiKey:_options.ApiKey);
        _rateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = _options.PermitLimit,
            Window = TimeSpan.FromSeconds(_options.WindowSeconds),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 100
        });
    }

    public async Task<ReviewResult> AnalyzeCodeToReviewAsync(string request)
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

            string cleanJson = ExtractJsonResponse(rawResponse);

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

    public void Dispose()
    {
        if (_disposed) return;

        _rateLimiter.Dispose();
        _client.Dispose();
        _disposed = true;
    }

    public string ExtractJsonResponse(string rawResponse)
    {
        Regex match = new Regex(@"\{.*\}", RegexOptions.Singleline);

        Match result = match.Match(rawResponse);

        if(result.Success)
           return result.Value;

        return string.Empty;
    }

}
