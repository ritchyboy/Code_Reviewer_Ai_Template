using CodeReviewerAI.Services.IStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services.Strategy
{
    public class CSharpLanguageStrategy : FileBasedLanguageStrategy
    {
        public override string[] SupportedExtensions => [".cs"];
        protected override string ConfigFileName => "Lang_CSharp.txt";
    }
}
