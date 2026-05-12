using CodeReviewerAI.Services.IStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.Strategy
{
    public class LanguageStrategy
    {
        public readonly Dictionary<string[], ILanguageStrategy> _strategies;

        public LanguageStrategy(IEnumerable<ILanguageStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(x => x.language);
        }
        public async Task<string> languageStrategy(string key)
        {
            var select = from val in _strategies
                         where _strategies.Keys.Any(x => x.Any(x => x.Equals(key)))
                         select val.Value.languagePromptSelection();
            return select.ToString();
        }
    }
}
