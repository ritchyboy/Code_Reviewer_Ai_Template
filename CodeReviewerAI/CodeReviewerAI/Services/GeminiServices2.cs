using System;
using System.Threading.Tasks;
using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using Google.GenAI;
using Google.GenAI.Types;
using Environment = System.Environment;


public class GeminiServices2: IGeminiServices2
{
    public string geminiModel = "gemini-3-flash-preview";
	private readonly Client _client;
    private readonly string _GeminiApiKey = GetApiKey();

	public GeminiServices2(string key)
	{
		_client = new Client(apiKey: key);
	}
	public async Task<ReviewResult> AnalyzeCodeToReview(string reviewCode)
	{
        var response = await _client.Models.GenerateContentAsync(
        model:geminiModel,contents:"Say hello twice"
    );
		string ?aiText = response.Candidates[0].Content.Parts[0].Text;
		if (string.IsNullOrEmpty(aiText))
		{
            Console.WriteLine("Gemini did not respond at all");
		}

        return new ReviewResult(aiText,40);
    }

}
