using CodeReviewerAI.Services.IStrategy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewerAI.Tests.Integration
{
    public class LanguageStrategyProviderTest : BaseIntegrationTest
    {
       
        [Fact]
        public void can_be_build_with_strategies()
        {
            var languageProvider = serviceProvider.GetRequiredService<ILanguageStrategyProvider>();

            Assert.NotNull(languageProvider);
        }
        [Theory]
        [InlineData(".cs")]
        [InlineData(".CS")]
        public async Task return_csharp_strategy_upper_to_lowercase(string ext)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Lang_CSharp.txt");
            string languageFile = await File.ReadAllTextAsync(path);

            var languageProvider = serviceProvider.GetRequiredService<ILanguageStrategyProvider>();

            var result = await languageProvider.LanguageStrategyImplementation(ext);

            Assert.Equal(result, languageFile);
        }
        [Theory]
        [InlineData(".cpp")]
        [InlineData(".CPP")]
        [InlineData(".h")]
        [InlineData(".H")]
        [InlineData(".hpp")]
        [InlineData(".HPP")]
        public async Task return_cpp_strategy_upper_to_lowercase(string ext)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Lang_CPP.txt");
            string languageFile = await File.ReadAllTextAsync(path);

            var languageProvider = serviceProvider.GetRequiredService<ILanguageStrategyProvider>();

            var result = await languageProvider.LanguageStrategyImplementation(ext);

            Assert.Equal(result, languageFile);
        }
        [Theory]
        [InlineData(".notsupported")]
        public async Task not_supported_exception_return_when_extension_is_not_supported(string ext)
        {
            var languageProvider = serviceProvider.GetRequiredService<ILanguageStrategyProvider>();

            Func<Task> result = () => languageProvider.LanguageStrategyImplementation(ext);

            await result.Should().ThrowAsync<NotSupportedException>();

        }
    }
}
