using CodeReviewerAI.Services.IStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.Strategy
{
    public class CSharpLanguageStrategy : ILanguageStrategy
    {
        string[] ILanguageStrategy.language => [".cs"];
        string basePath = AppDomain.CurrentDomain.BaseDirectory;


        public async Task<string> languagePromptSelection()
        {
            string path = Path.Combine(basePath, "Config", "Lang_CSharp.txt");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            string languagePrompt = await File.ReadAllTextAsync(path);

            return languagePrompt;
        }
    }
}
