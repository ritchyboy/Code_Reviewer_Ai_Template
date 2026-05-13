using CodeReviewerAI.Services.IServices;
using CodeReviewerAI.Services.IStrategy;
using System.Text;

namespace CodeReviewerAI.Services
{
    public class PromptService: IPromptService
    {
        private readonly IStrategyLanguage _strategyLanguage;
        private readonly string baseApplicationPath = AppDomain.CurrentDomain.BaseDirectory;

        public PromptService(IStrategyLanguage strategyLanguage)
        {
            _strategyLanguage = strategyLanguage;
        }
        public async Task<string> getBasePromptAsync()
        {
            string basePrompt = string.Empty;
            string basePromptPath = Path.Combine(baseApplicationPath, "Config", "Base_Persona.txt");

            if (!File.Exists(basePromptPath))
            {
                throw new FileNotFoundException("Base_Persona.txt was not found in the Config folder");
            }
            basePrompt = await File.ReadAllTextAsync(basePromptPath);

            return basePrompt;
        }

        public async Task<string> getOutputSchemaPromptAsync()
        {
            string outputPrompt = string.Empty;
            string output_Schema_Path = Path.Combine(baseApplicationPath, "Config", "Output_Schema.txt");

            if (!File.Exists(output_Schema_Path))
            {
                throw new FileNotFoundException("Output_Schema.txt was not found in the Config Folder");
            }
            outputPrompt = await File.ReadAllTextAsync(output_Schema_Path);

            return outputPrompt;
        }
        public async Task<string> languageManagerPromptAsync(string fileExt)
        {
            string ext = Path.GetExtension(fileExt);
            string result = await _strategyLanguage.LanguageStrategyImplementation(ext);

            return result;
          /*  string languagePrompt = string.Empty;
            string csharp_Lang_Path = Path.Combine(baseApplicationPath,"Config","Lang_CSharp.txt");
            string cpp_Lang_Path = Path.Combine(baseApplicationPath,"Config","Lang_CPP.txt");

            if(!File.Exists(csharp_Lang_Path)||!File.Exists(cpp_Lang_Path))
            {
                throw new FileNotFoundException("Language file was not found in the Config folder");
            }

            if (fileExt.ToLower().EndsWith(".cs"))
            {
                languagePrompt = await File.ReadAllTextAsync(csharp_Lang_Path);
            }
            else
            {
                languagePrompt = await File.ReadAllTextAsync(cpp_Lang_Path);
            }

            return languagePrompt;
          */
        }

        public async Task<string> promptManagerAsync(string fileExt,string codeSample)
        {
         
            ArgumentException.ThrowIfNullOrWhiteSpace(codeSample, nameof(codeSample));
            
            var getBasePromptTask = getBasePromptAsync();
            var getLangPromptTask =  languageManagerPromptAsync(fileExt);
            var getOutputPromptTask = getOutputSchemaPromptAsync();


            await Task.WhenAll(getBasePromptTask, getLangPromptTask, getOutputPromptTask);

            int estimatedSize = 1200 + codeSample.Length;
            StringBuilder fullPromptBuilder = new StringBuilder(estimatedSize);
       
            fullPromptBuilder.AppendLine(getBasePromptTask.Result);
            fullPromptBuilder.AppendLine(getLangPromptTask.Result);
            fullPromptBuilder.AppendLine(getOutputPromptTask.Result);
            fullPromptBuilder.AppendLine(codeSample);
            
            return fullPromptBuilder.ToString();


        }
    }
}
