namespace CodeReviewerAI.Services.IServices
{
    public interface IPromptService
    {
        // Manage the config file to match return the appropriate prompt
        public Task<string> GetCompletePromptAsync(string fileExt,string codeSample);
        public Task<string> GetBasePromptAsync();
        public Task<string> GetOutputSchemaPromptAsync();
        public Task<string> GetLanguagePromptAsync(string fileExt);
    }
}
