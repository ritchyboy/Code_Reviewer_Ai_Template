using CodeReviewerAI.Models;
using Octokit;
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
        public Task createReviewCommentAsync(string owner, string repoName, int prNumber,ReviewResult pullRequestComment);
    }
}
