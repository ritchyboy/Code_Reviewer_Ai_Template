using CodeReviewerAI.Services.IStrategy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.Strategy
{
    public class CppLanguageStrategy : FileBasedLanguageStrategy
    {
        public override string[] SupportedExtensions => [".cpp", ".h", ".hpp"];
        protected override string ConfigFileName => "Lang_CPP.txt";
    }
}
