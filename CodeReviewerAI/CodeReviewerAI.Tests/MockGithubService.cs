using CodeReviewerAI.Services;
using CodeReviewerAI.Services.Github;
using FluentAssertions;
using Google.Apis.Util;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewerAI.Tests
{
    public class MockGithubService
    {
        [Fact]
        public void Initiate_Mock_Github_Options_Service()
        {
            var mockOption = Options.Create(new GithubOptions
            {
                Token = "Alfredisatoken",
                AppName = "app"
            });

            var service = new GithubServices(mockOption);

            service.Should().NotBeNull();
        }
        [Fact]
        public void Mock_Github_Options_Return_ArgumentException()
        {
            var mockOptions = Options.Create(new GithubOptions
            {
                AppName = "",
                Token = ""
            });

            Assert.Throws<ArgumentException>(() => new GithubServices(mockOptions));
        }
        [Theory]
        [InlineData("app","")]
        [InlineData("", "something")]
        public void Mock_Github_Options_Return_ArgumentException_On_Scenario(string appName,string token)
        {
            var mockOptions = Options.Create(new GithubOptions
            {
                AppName = appName,
                Token = token
            });

            Assert.Throws<ArgumentException>(() => new GithubServices(mockOptions));
        }

    }
}
