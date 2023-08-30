using HackerNewsAPI.Models;

namespace HackerNewsAPI.Services
{
    public interface IHackerNewsService
    {
        Task<GetBestStoriesResponse> GetBestStoriesAsync(ushort numberOfStories, CancellationToken cancellationToken);
    }
}