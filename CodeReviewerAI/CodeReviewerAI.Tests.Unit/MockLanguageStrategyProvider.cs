using CodeReviewerAI.Services.IStrategy;
using CodeReviewerAI.Services.Strategy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.AI;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace CodeReviewerAI.Tests.Unit
{
    public class MockLanguageStrategyProvider
    {
        [Fact]
        public void initiate_language_strategy_provider()
        {
            var mockListStrategy = new List<ILanguageStrategy>() { new CSharpLanguageStrategy(),new CppLanguageStrategy()};
            var languageStrategy = new LanguageStrategyProvider(mockListStrategy);
            var result = languageStrategy.LanguageStrategyImplementation(".cs");

            languageStrategy.Should().NotBeNull();
        }

        [Fact]
        public void should_return_not_supported_error()
        {
            var mockListStrategy = new List<ILanguageStrategy>() { new CSharpLanguageStrategy(),
            new CppLanguageStrategy()};

            var languageStrategy = new LanguageStrategyProvider(mockListStrategy);


            Func<Task> result = async () => await languageStrategy.LanguageStrategyImplementation(".py");
            result.Should().ThrowAsync<NotSupportedException>();
        }

    }
}
