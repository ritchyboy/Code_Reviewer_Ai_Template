using CodeReviewerAI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Services
{
    public class PromptService: IPromptService
    {
        private readonly string baseApplicationPath = AppDomain.CurrentDomain.BaseDirectory;

        public string getBasePrompt()
        {
            string basePrompt = string.Empty;
            string basePromptPath = Path.Combine(baseApplicationPath, "Config", "Base_Persona.txt");

            if (!File.Exists(basePromptPath))
            {
                throw new FileNotFoundException("Base_Persona.txt was not found in the Config folder");
            }
            basePrompt = File.ReadAllText(basePromptPath);

            return basePrompt;
        }

        public string getOutputSchemaPrompt()
        {
            string outputPrompt = string.Empty;
            string output_Schema_Path = Path.Combine(baseApplicationPath, "Config", "Output_Schema.txt");

            if (!File.Exists(output_Schema_Path))
            {
                throw new FileNotFoundException("Output_Schema.txt was not found in the Config Folder");
            }
            outputPrompt = File.ReadAllText(output_Schema_Path);

            return outputPrompt;
        }
        public string languageManagerPrompt(string fileExt)
        {
            string languagePrompt = string.Empty;
            string csharp_Lang_Path = Path.Combine(baseApplicationPath,"Config","Lang_CSharp.txt");
            string cpp_Lang_Path = Path.Combine(baseApplicationPath,"Config","Lang_CPP.txt");

            if(!File.Exists(csharp_Lang_Path)||!File.Exists(cpp_Lang_Path))
            {
                throw new FileNotFoundException("Language file was not found in the Config folder");
            }

            if (fileExt.ToLower().EndsWith(".cs"))
            {
                languagePrompt = File.ReadAllText(csharp_Lang_Path);
            }
            else
            {
                languagePrompt = File.ReadAllText(cpp_Lang_Path);
            }

            return languagePrompt;
        }

        public async Task<string> promptManager(string fileExt)
        {
            string fullPrompt = getBasePrompt()+ "\n" + languageManagerPrompt(fileExt)
             +"\n" + getOutputSchemaPrompt();

            return fullPrompt;
        }
    }
}
