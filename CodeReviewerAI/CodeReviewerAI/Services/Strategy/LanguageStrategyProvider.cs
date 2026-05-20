using CodeReviewerAI.Services.IStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.Strategy
{
    public class LanguageStrategyProvider : ILanguageStrategyProvider
    {
        private readonly IEnumerable<ILanguageStrategy> _strategies;

        public LanguageStrategyProvider(IEnumerable<ILanguageStrategy> strategies)
        {
            _strategies = strategies;
        }
        public bool HasStrategy(string extension)
        {
            return _strategies.Any(x => x.IsExtensionSupported(extension));
        }
        public async Task<string> LanguageStrategyImplementation(string extension)
        {
            var strategy = _strategies.FirstOrDefault(x => x.IsExtensionSupported(extension));
            if(strategy == null)
            throw new NotSupportedException($"No strategy for {extension}");

            return await strategy.LanguagePromptSelection();
        }
    }
}
