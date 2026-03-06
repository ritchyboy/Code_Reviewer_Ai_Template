using CodeReviewerAI.Models;
using CodeReviewerAI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        public ReviewerService(IGeminiServices geminiServices,IGithubServices githubServices,
        IPromptService promptService)
        {
            _geminiServices = geminiServices;
            _githubServices = githubServices;
            _promptService = promptService;

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

            if (!isFileExtensionMatch)
            {
                // Need a LinQ Group that group element by extention so c++ file with together ts together etc
                var groupedByExtension = from file in fileList
                                         group file by Path.GetExtension(file.fileName).ToLower() into g
                                         select new { Ext = g.Key, Files = g };
                                         

                foreach (var groupe in groupedByExtension)
                {
                    StringBuilder groupOfFile = new StringBuilder();
                }
                return pullrequestComment;
            }
            else
            {
                string firstFile = fileList[0].fileName;

                foreach (var file in fileList)
                {
                    reviewCode += file.fileName + file.patch + "\n";
                }
                string fullPrompt = await _promptService.promptManagerAsync(firstFile, reviewCode);

                pullrequestComment = await _geminiServices.AnalyzeCodeToReview(fullPrompt);

                return pullrequestComment;
            }

        }
    }
}
