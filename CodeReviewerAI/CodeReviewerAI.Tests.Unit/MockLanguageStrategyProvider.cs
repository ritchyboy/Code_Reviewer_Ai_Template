using CodeReviewerAI.Services.IStrategy;
using CodeReviewerAI.Services.Strategy;
using FluentAssertions;


namespace CodeReviewerAI.Tests.Unit
{
    public class MockLanguage : FileBasedLanguageStrategy
    {
        public override string[] SupportedExtensions => [".cs"];

        protected override string ConfigFileName => "Lang_CSharp.txt";
    }
    public class MockLanguageFakeFile : FileBasedLanguageStrategy
    {
        public override string[] SupportedExtensions => [".cs"];

        protected override string ConfigFileName => "Lang_File.txt";
    }
    public class MockLanguageStrategyProvider
    {
        [Fact]
        public async Task Initiate_LanguageStrategyProvider()
        {
            var mockListStrategy = new List<ILanguageStrategy>() { new MockLanguage()};
            var languageStrategy = new LanguageStrategyProvider(mockListStrategy);
            var result = await languageStrategy.LanguageStrategyImplementation(".cs");

            result.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public async Task Return_FileNotFound_Error()
        {
            var mockListStrategy = new List<ILanguageStrategy>() { new MockLanguageFakeFile() };
            var languageStrategy = new LanguageStrategyProvider(mockListStrategy);
            Func<Task> result = () => languageStrategy.LanguageStrategyImplementation(".cs");

            await result.Should().ThrowAsync<FileNotFoundException>();

        }

        [Fact]
        public async Task should_return_not_supported_error()
        {
            var mockListStrategy = new List<ILanguageStrategy>() { new MockLanguage()};

            var languageStrategy = new LanguageStrategyProvider(mockListStrategy);


            Func<Task> result = async () => await languageStrategy.LanguageStrategyImplementation(".py");
            await result.Should().ThrowAsync<NotSupportedException>();
        }

    }
}
