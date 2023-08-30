namespace HackerNewsAPI.ExternalApi
{
    public interface IHackerNewsApiClient
    {
        Task<ICollection<int>?> GetBestStoriesAsync(CancellationToken cancellationToken);
        Task<HackerNewsItem?> GetItemAsync(int id, CancellationToken cancellationToken);
    }
}