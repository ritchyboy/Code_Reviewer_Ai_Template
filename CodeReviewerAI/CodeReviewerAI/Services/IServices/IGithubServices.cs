using CodeReviewerAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.IServices
{
    public interface IGithubServices
    {
        public Task<List<GithubFileChange>> pullRequestDiffs(string owner,string repoName,int prNumber);

        public string getDataFromUser();
    }
}
