using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.IStrategy
{
    public interface ILanguageStrategy
    {
        string[] language { get;}

        public Task<string> languagePromptSelection();
    }
}
