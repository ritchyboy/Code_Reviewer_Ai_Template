using CodeReviewerAI.Services.IStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.Strategy
{
    public abstract class FileBasedLanguageStrategy : ILanguageStrategy
    {
        protected abstract string ConfigFileName { get; }
        public abstract string[] SupportedExtensions { get; }

        public async Task<string> LanguagePromptSelection()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config",ConfigFileName);

            if (!File.Exists(path))
            throw new FileNotFoundException(path);

            return await File.ReadAllTextAsync(path);
        }
        public bool IsExtensionSupported(string extension)
        {
            return SupportedExtensions.Contains(extension,StringComparer.OrdinalIgnoreCase);
        }
    }
}
