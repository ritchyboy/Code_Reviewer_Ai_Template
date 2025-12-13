using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services
{
    public class GithubServices
    {
        private string _GITHUB_API_KEY;
        string githubUrl = @"https://api.github.com/repos/OWNER/REPO/pulls/PULL_NUMBER/comments";
        public void PostComment(string comment)
        {
            StringContent content = new StringContent(comment);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer <{_GITHUB_API_KEY}>");
                client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
                client.PostAsync(githubUrl, content);
            }
        }

        public string SET_API_KEY(string GITHUBAPIKEY)
        {
            return _GITHUB_API_KEY = GITHUBAPIKEY; 
        }
    }
}
