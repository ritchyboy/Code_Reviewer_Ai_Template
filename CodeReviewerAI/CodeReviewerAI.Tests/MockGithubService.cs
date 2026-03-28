using CodeReviewerAI.Services;
using CodeReviewerAI.Services.Github;
using FluentAssertions;
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
    }
}
