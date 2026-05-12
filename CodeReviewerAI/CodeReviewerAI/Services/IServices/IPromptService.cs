namespace CodeReviewerAI.Services.IServices
{
    public interface IPromptService
    {
        // Manage the config file to match return the appropriate prompt
        public Task<string> promptManagerAsync(string fileExt,string codeSample);
        public Task<string> getBasePromptAsync();
        public Task<string> getOutputSchemaPromptAsync();
        public Task<string> languageManagerPromptAsync(string fileExt);
    }
}
