using System.Collections.Concurrent;
using System.Net;
using HackerNewsAPI.ExternalApi;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNewsAPI.IntegrationTests.Tests
{
    public class BestStoriesTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;

        public BestStoriesTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "When subsequent clients request less number of best stories than first client, cached value is used.")]
        public async void GetBestStories()
        {
            // Arrange
            const int numberOfApiClients = 10;
            const int firstClientNumberOfStories = 100;
            var fakeHackerNewsApiClient = new FakeHackerNewsApiClient();
            var responses = new ConcurrentBag<HttpResponseMessage?>();
            var apiClient = _factory.WithWebHostBuilder(x =>
            {
                x.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IHackerNewsApiClient>(fakeHackerNewsApiClient);
                });
            }).CreateClient();


            // Act            
            var firstResponse = await apiClient.GetAsync($"best-stories?numberOfStories={firstClientNumberOfStories}");
            responses.Add(firstResponse);

            await Parallel.ForEachAsync(Enumerable.Range(1, numberOfApiClients), async (clientId, ct) =>
            {
                var n = firstClientNumberOfStories - clientId;
                var response = await apiClient.GetAsync($"best-stories?numberOfStories={n}", ct);
                responses.Add(response);
            });

            // Assert
            foreach(var response in responses) 
            {
                Assert.NotNull(response);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            Assert.Equal(
                1, // Expected number of calls to HackerNews API
                fakeHackerNewsApiClient.BestStoriesAsyncCallCounter);
        }
    }
}