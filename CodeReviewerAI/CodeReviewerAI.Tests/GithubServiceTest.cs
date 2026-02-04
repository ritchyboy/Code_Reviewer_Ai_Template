using CodeReviewerAI.Services;

namespace CodeReviewerAI.Tests.Services
{
    public class GithubServiceTest
    {

        [Fact]
        public void services_can_be_constructed()
        {
            // Arrange
            string appName = "CodeReviewerAI";

            // Act
            var services = new GithubServices(appName);

            // Assert
            Assert.NotNull(services);
        }

    }
}
