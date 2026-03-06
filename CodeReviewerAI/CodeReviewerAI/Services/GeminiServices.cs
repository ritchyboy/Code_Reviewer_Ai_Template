using CodeReviewerAI.Models;
using CodeReviewerAI.Services.IServices;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;


public class GeminiServices: IGeminiServices
{
    public string Provider => "Google";
    public string ModelName => "gemini-3-flash-preview";
    public double Temperature => 1.0;
	private readonly Client _client;

	public GeminiServices(string key)
	{
		_client = new Client(apiKey: key);
	}

	public async Task<ReviewResult> AnalyzeCodeToReview(string promptWithCode)
	{
        var response = await _client.Models.GenerateContentAsync(
        model:ModelName,contents:promptWithCode
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
