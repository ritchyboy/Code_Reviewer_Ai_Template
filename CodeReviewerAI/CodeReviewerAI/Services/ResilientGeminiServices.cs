using CodeReviewerAI.Models;
using CodeReviewerAI.Services.IServices;
using Polly.Registry;


namespace CodeReviewerAI.Services
{
    public class ResilientGeminiServices : IGeminiServices
    {
        private readonly IGeminiServices _geminiService;
        private readonly ResiliencePipelineProvider<string> _resiliencePipeline;

        public ResilientGeminiServices(IGeminiServices geminiServices,
        ResiliencePipelineProvider<string> resiliencePipelineProvider)
        {
            _geminiService = geminiServices;
            _resiliencePipeline = resiliencePipelineProvider;
        }
        public async Task<ReviewResult> AnalyzeCodeToReview(string request)
        {
            var pipeline = _resiliencePipeline.GetPipeline("Default");
            return await pipeline.ExecuteAsync(async ct => await _geminiService.AnalyzeCodeToReview(request));    
        }
    }
}
