using System;
using System.Threading.Tasks;
using CodeReviewerAI.Models;
using CodeReviewerAI.Services.IServices;
using Google.GenAI;
using Google.GenAI.Types;


public class GeminiServices2: IGeminiServices2
{
    public string geminiModel = "gemini-3-flash-preview";
	private readonly Client _client;

	public GeminiServices2(string key)
	{
		_client = new Client(apiKey: key);
	}
	public async Task<ReviewResult> AnalyzeCodeToReview(string reviewCode)
	{
        var response = await _client.Models.GenerateContentAsync(
        model:geminiModel,contents:"Say hello twice"
    );
		string aiText = response.Candidates[0].Content.Parts[0].Text;
		var reviewRespond = new ReviewResult();
		reviewRespond.MarkdownReview = aiText;
		
		if (string.IsNullOrEmpty(aiText))
		{
            Console.WriteLine("Gemini did not respond at all");
		}

        return reviewRespond;
    }

}
