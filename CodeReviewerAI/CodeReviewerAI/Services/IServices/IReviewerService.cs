using CodeReviewerAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.IServices
{
    public interface IReviewerService
    {
        public Task<ReviewResult> ReviewPullrequestAsync(string owner, string reposName, int prNumber);
        public Task<ReviewResult> RunAsync(string owner, string reposName, int prNumber);
    }
}
