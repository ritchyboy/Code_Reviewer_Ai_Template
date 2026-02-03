using System;
namespace CodeReviewerAI.Services.IServices
{

     public interface IGeminiServices2
     {
         public Task<ReviewResult> AnalyzeCodeToReview(string reviewCode);
     }
} 
