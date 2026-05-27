using CodeReviewerAI.Models;
using CodeReviewerAI.Services.IServices;
using System.Data;
using System.Text;


namespace CodeReviewerAI.Services
{
    public class ReviewerService : IReviewerService
    {
        private readonly IGeminiServices _geminiServices;
        private readonly IGithubServices _githubServices;
        private readonly IPromptService _promptService;

         public ReviewerService(IGeminiServices geminiServices,IGithubServices githubServices
         ,IPromptService promptService)
         {
             _geminiServices = geminiServices;
             _githubServices = githubServices;
             _promptService = promptService;
         }
        public async Task<ReviewResult> RunAsync(string owner,string reposName,int prNumber)
        {
            var result = await ReviewPullrequestAsync(owner, reposName, prNumber);
            return result;
        }
        public async Task<ReviewResult> ReviewPullrequestAsync(string owner,string reposName,int prNumber)
        {
            StringBuilder pullRequestDiffs = new StringBuilder();
            StringBuilder listOfRequest = new StringBuilder();

            string basePrompt = await _promptService.GetBasePromptAsync();

            listOfRequest.AppendLine(basePrompt);
            ReviewResult pullrequestComment = new ReviewResult();

            var fileList = new List<GithubFileChange>();
            fileList = await _githubServices.GetPullRequestDiffsAsync(owner, reposName, prNumber);
            if(fileList.Count == 0)
            {
                throw new InvalidOperationException("An empty pullrequest cannot be review");
            }
            string firstExtension = Path.GetExtension(fileList[0].FileName);

            bool isFileExtensionMatch = fileList.All(ext => Path.GetExtension(ext.FileName)
            .Equals(firstExtension,StringComparison.OrdinalIgnoreCase));

            if (!isFileExtensionMatch)
            {
                // Need a LinQ Group that group element by extention so c++ file with together ts together etc
                var groupedByExtension = from file in fileList
                                         group file by Path.GetExtension(file.FileName).ToLower() into g
                                         select new { Ext = g.Key, Files = g };
                                         
                
                foreach (var group in groupedByExtension)
                {
                    StringBuilder fileGroup = new StringBuilder();
                    List<GithubFileChange> fileChange = group.Files.ToList();
                    foreach(var file in fileChange)
                    {
                        fileGroup.Append(file.FileName);
                        fileGroup.AppendLine(file.Patch);
                    }
                    string request = await _promptService.GetLanguagePromptAsync(group.Ext);
                    listOfRequest.AppendLine(request);
                    pullRequestDiffs.AppendLine(fileGroup.ToString());
                    fileGroup.Clear();
                }
                string outputSchemas = await _promptService.GetOutputSchemaPromptAsync();
                listOfRequest.AppendLine(outputSchemas);
                listOfRequest.AppendLine(pullRequestDiffs.ToString());


                pullrequestComment = await _geminiServices.AnalyzeCodeToReviewAsync(listOfRequest.ToString());
                return pullrequestComment;
            }
            else
            {
                string firstFile = fileList[0].FileName;

                foreach (var file in fileList)
                {
                    listOfRequest.Append(file.FileName);
                    listOfRequest.AppendLine(file.Patch);
                }
                string fullPrompt = await _promptService.GetCompletePromptAsync(firstFile, listOfRequest.ToString());

                pullrequestComment = await _geminiServices.AnalyzeCodeToReviewAsync(fullPrompt);

                return pullrequestComment;
            }

        }
    }
}
