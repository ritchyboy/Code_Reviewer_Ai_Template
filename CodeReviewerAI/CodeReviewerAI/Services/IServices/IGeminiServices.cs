using CodeReviewerAI.Models;
namespace CodeReviewerAI.Services.IServices
{
     public interface IGeminiServices
     {
         public Task<ReviewResult> AnalyzeCodeToReviewAsync(string request);
     }
} 
