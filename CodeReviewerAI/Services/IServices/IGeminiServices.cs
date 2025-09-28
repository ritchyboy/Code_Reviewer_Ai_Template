using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.IServices
{
    public interface IGeminiServices
    {
        public string InitPrompt(string message);
        public Task<string> GetReviewFromCode(string codeToReview);
    }
}
