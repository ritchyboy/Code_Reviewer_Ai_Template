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
            StringBuilder listOfRequest = new StringBuilder();
            ReviewResult pullrequestComment = new ReviewResult();
            string reviewCode = string.Empty;

            var fileList = new List<GithubFileChange>();
            fileList = await _githubServices.GetPullRequestDiffsAsync(owner, reposName, prNumber);
            if(fileList.Count == 0)
            {
                throw new NoNullAllowedException("An empty pullrequest cannot be review");
            }
            string firstExtension = Path.GetExtension(fileList[0].fileName);

            bool isFileExtensionMatch = fileList.All(ext => Path.GetExtension(ext.fileName)
            .Equals(firstExtension,StringComparison.OrdinalIgnoreCase));

            if (!isFileExtensionMatch)
            {
                // Need a LinQ Group that group element by extention so c++ file with together ts together etc
                var groupedByExtension = from file in fileList
                                         group file by Path.GetExtension(file.fileName).ToLower() into g
                                         select new { Ext = g.Key, Files = g };
                                         
                
                foreach (var groupe in groupedByExtension)
                {
                    StringBuilder fileGroup = new StringBuilder();
                    List<GithubFileChange> fileChange = groupe.Files.ToList();
                    foreach(var file in fileChange)
                    {
                        fileGroup.Append(file.fileName);
                        fileGroup.AppendLine(file.patch);
                    }
                    string fullRequest = await _promptService.GetCompletePromptAsync(groupe.Ext, fileGroup.ToString());
                    fileGroup.Clear();
                    listOfRequest.AppendLine(fullRequest);
                }
                pullrequestComment = await _geminiServices.AnalyzeCodeToReviewAsync(listOfRequest.ToString());
                return pullrequestComment;
            }
            else
            {
                string firstFile = fileList[0].fileName;

                foreach (var file in fileList)
                {
                    reviewCode += file.fileName + file.patch + "\n";
                }
                string fullPrompt = await _promptService.GetCompletePromptAsync(firstFile, reviewCode);


                pullrequestComment = await _geminiServices.AnalyzeCodeToReviewAsync(fullPrompt);

                return pullrequestComment;
            }

        }
    }
}
