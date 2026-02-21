using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.IServices
{
    public interface IPromptService
    {
        // Manage the config file to match return the appropriate prompt
        public Task<string> promptManager(string fileExt,string codeSample);
        public string getBasePrompt();
        public string getOutputSchemaPrompt();
        public string languageManagerPrompt(string fileExt);
    }
}
