using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.IServices
{
    public interface IGithubServices
    {
        public Task<List<string>> pullRequestDiffs(string owner,string repoName,int prNumber);
    }
}
