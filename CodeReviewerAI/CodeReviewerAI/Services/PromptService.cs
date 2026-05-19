using CodeReviewerAI.Services.IServices;
using CodeReviewerAI.Services.IStrategy;
using System.Text;

namespace CodeReviewerAI.Services
{
    public class PromptService: IPromptService
    {
        private readonly IStrategyLanguage _strategyLanguage;
        private readonly IPromptService _promptService;
        private readonly string baseApplicationPath = AppDomain.CurrentDomain.BaseDirectory;

        public PromptService(IStrategyLanguage strategyLanguage)
        {
            _strategyLanguage = strategyLanguage;
        }
        public async Task<string> GetBasePromptAsync()
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

        public async Task<string> GetOutputSchemaPromptAsync()
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
        public async Task<string> GetLanguagePromptAsync(string fileExt)
        {
            string ext = Path.GetExtension(fileExt);
            string result = await _strategyLanguage.LanguageStrategyImplementation(ext);

            return result;
        }

        public async Task<string> GetCompletePromptAsync(string fileExt,string codeSample)
        {
         
            ArgumentException.ThrowIfNullOrWhiteSpace(codeSample, nameof(codeSample));
            Task<string> getBasePromptTask = _promptService.GetBasePromptAsync();
            Task<string> getLangPromptTask = _promptService.GetLanguagePromptAsync(fileExt);
            Task<string> getOutputPromptTask = _promptService.GetOutputSchemaPromptAsync();


            await Task.WhenAll(getBasePromptTask, getLangPromptTask, getOutputPromptTask);

            string BasePrompt = await getBasePromptTask;
            string LangPrompt =  await getLangPromptTask;
            string OutputPrompt = await getOutputPromptTask;



            int estimatedSize = 1200 + codeSample.Length;
            StringBuilder fullPromptBuilder = new StringBuilder(estimatedSize);
       
            fullPromptBuilder.AppendLine(BasePrompt);
            fullPromptBuilder.AppendLine(LangPrompt);
            fullPromptBuilder.AppendLine(OutputPrompt);
            fullPromptBuilder.AppendLine(codeSample);
            
            return fullPromptBuilder.ToString();

        }
    }
}
