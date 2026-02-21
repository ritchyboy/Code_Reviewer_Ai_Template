using CodeReviewerAI.Models;
using CodeReviewerAI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services
{
    public class ReviewerService : IReviewerService
    {
        private IGeminiServices _geminiServices;
        private IGithubServices _githubServices;
        private IPromptService _promptService;

        public ReviewerService(IGeminiServices geminiServices,IGithubServices githubServices)
        {
            _geminiServices = geminiServices;
            _githubServices = githubServices;
        }
        public async Task<ReviewResult> Run(string owner,string reposName,int prNumber)
        {
            var result = await ReviewPullrequestAsync(owner, reposName, prNumber);
            return result;
        }
        public async Task<ReviewResult> ReviewPullrequestAsync(string owner,string reposName,int prNumber)
        {
            ReviewResult pullrequestComment = new ReviewResult();
            string reviewCode = string.Empty;

            var fileList = new List<GithubFileChange>();
            fileList = await _githubServices.pullRequestDiffs(owner, reposName, prNumber);
            if(fileList.Count == 0)
            {
                throw new NoNullAllowedException("An empty pullrequest cannot be review");
            }
            bool isFileExtensionMatch = fileList.All(ext => ext.fileName.
            EndsWith(new FileInfo(ext.fileName).Extension));

            if (isFileExtensionMatch)
            {
                string firstFile = fileList[0].fileName;
                var promptService = new PromptService();

                foreach (var file in fileList)
                {
                    reviewCode += file.fileName + file.patch + "\n";
                }
                string fullPrompt = await promptService.promptManager(firstFile,reviewCode);

                pullrequestComment = await _geminiServices.AnalyzeCodeToReview(fullPrompt);

                return pullrequestComment;
            }
            return pullrequestComment;

        }
    }
}
