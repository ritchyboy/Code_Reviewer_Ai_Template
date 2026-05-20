using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.IStrategy
{
    public interface ILanguageStrategyProvider
    {
        public Task<string> LanguageStrategyImplementation(string extension);
        bool HasStrategy(string extension);
    }
}
