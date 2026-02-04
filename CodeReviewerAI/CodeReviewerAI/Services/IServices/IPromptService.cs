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
        public string promptManager(FileInfo info);
        public string getBasePrompt();
        public string getOutputSchemaPrompt();
        public string languageManagerPrompt(FileInfo fileInfo);
    }
}
