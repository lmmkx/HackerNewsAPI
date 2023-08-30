using HackerNewsAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Services
{
    public class MemoryCachedHackerNewsService : IHackerNewsService
    {
        private const string CacheKey = "HackerNewsBestStories";
        private static readonly SemaphoreSlim CacheWriteSemaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        
        private readonly IHackerNewsService _hackerNewsService;
        private readonly IMemoryCache _memoryCache;

        public MemoryCachedHackerNewsService(IHackerNewsService hackerNewsService, IMemoryCache memoryCache)
        {
            _hackerNewsService = hackerNewsService;
            _memoryCache = memoryCache;
        }

        public async Task<GetBestStoriesResponse> GetBestStoriesAsync(ushort numberOfStories, CancellationToken cancellationToken)
        {
            GetBestStoriesResponse? cachedResponse = GetBestStoriesFromCacheOrNull(numberOfStories);
            if (cachedResponse != null)
                return cachedResponse;
            

            await CacheWriteSemaphore.WaitAsync(cancellationToken);
            try
            {
                cachedResponse = GetBestStoriesFromCacheOrNull(numberOfStories);
                if (cachedResponse != null) 
                    return cachedResponse;

                var serviceResponse = await _hackerNewsService.GetBestStoriesAsync(numberOfStories, cancellationToken);
                if (serviceResponse.IsError)
                    return serviceResponse; // Just forward, as there is no sense of caching invalid response

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // todo: read from config

                _memoryCache.Set(
                    key: CacheKey,
                    value: serviceResponse,
                    options: cacheEntryOptions);

                return serviceResponse;
            }
            finally
            {
                CacheWriteSemaphore.Release();
            }   
        }

        private GetBestStoriesResponse? GetBestStoriesFromCacheOrNull(int numberOfStories)
        {
            _memoryCache.TryGetValue(key: CacheKey, out GetBestStoriesResponse? cachedResponse);
            if (cachedResponse == null)            
                return null;            

            var cachedResponseHasEnoughStories = cachedResponse.Items.Count >= numberOfStories;
            return cachedResponseHasEnoughStories 
                ? new GetBestStoriesResponse(cachedResponse.Items.Take(numberOfStories).ToList()) 
                : null;
        }
    }
}
