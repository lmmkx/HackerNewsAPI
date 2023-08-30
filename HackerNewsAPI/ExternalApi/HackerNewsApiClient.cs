namespace HackerNewsAPI.ExternalApi
{
    internal class HackerNewsApiClient : IHackerNewsApiClient
    {
        private readonly HttpClient _httpClient;

        public HackerNewsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://hacker-news.firebaseio.com/"); // todo: read from config
        }

        public async Task<ICollection<int>?> GetBestStoriesAsync(CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<ICollection<int>>("/v0/beststories.json", cancellationToken);
        }

        public async Task<HackerNewsItem?> GetItemAsync(int id, CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<HackerNewsItem>($"/v0/item/{id}.json", cancellationToken);
        }
    }
}
