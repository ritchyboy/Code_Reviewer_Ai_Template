using CodeReviewerAI.Models;
using CodeReviewerAI.Services;
using CodeReviewerAI.Services.IServices;
using CodeReviewerAI.Tests.Integration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeReviewerAI.Tests.Services
{
    public class GithubServiceTest : BaseIntegrationTest
    {

        [Fact]
        public void services_can_be_constructed()
        {
            // Act
            var services = serviceProvider.GetRequiredService<IGithubServices>() ;

            // Assert
            services.Should().NotBeNull();   
        }

        [Fact]
        public void get_data_from_service()
        {
            var service = serviceProvider.GetRequiredService<IGithubServices>();

            string data = service.getDataFromUser();

            data.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public async Task Analyze_PullRequest_Diff_For_Response_From_Github()
        {
            string owner = "ritchyboy";
            string reposName = "SandBox_Test";


            var service = serviceProvider.GetRequiredService<IGithubServices>();
            var diffs = new List<GithubFileChange>();
            diffs = await service.pullRequestDiffs(owner,reposName,1);

            diffs.Should().NotBeNull();
        }
    }
}
