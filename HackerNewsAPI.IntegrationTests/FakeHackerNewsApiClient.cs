using HackerNewsAPI.ExternalApi;

namespace HackerNewsAPI.IntegrationTests
{
    internal class FakeHackerNewsApiClient : IHackerNewsApiClient
    {
        private int _getBestStoriesAsyncCallCounter;
        public int BestStoriesAsyncCallCounter { get { return _getBestStoriesAsyncCallCounter; } }

        public Task<ICollection<int>?> GetBestStoriesAsync(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _getBestStoriesAsyncCallCounter);

            var ids = Enumerable.Range(0, 500).ToList();
            return Task.FromResult((ICollection<int>?)ids);
        }

        public Task<HackerNewsItem?> GetItemAsync(int id, CancellationToken cancellationToken)
        {
            var item = new HackerNewsItem("Title 1", "https://www.test1.com", "User1", 1175714200, 111, 71);
            return Task.FromResult(item) as Task<HackerNewsItem?>;
        }
    }
}
