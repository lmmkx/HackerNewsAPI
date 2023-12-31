﻿using System.Collections.Concurrent;
using HackerNewsAPI.ExternalApi;
using HackerNewsAPI.Models;

namespace HackerNewsAPI.Services
{
    public sealed class HackerNewsService : IHackerNewsService
    {
        private readonly IHackerNewsApiClient _hackerNewsApiClient;

        public HackerNewsService(IHackerNewsApiClient hackerNewsApiClient)
        {
            _hackerNewsApiClient = hackerNewsApiClient;
        }

        public async Task<GetBestStoriesResponse> GetBestStoriesAsync(ushort numberOfStories, CancellationToken cancellationToken)
        {
            var bestStoriesIds = await _hackerNewsApiClient.GetBestStoriesAsync(cancellationToken);
            if (bestStoriesIds == null)
                return new GetBestStoriesResponse(errorMessage: "Error while retrieving best stories.");

            var bestStories = new ConcurrentBag<Story>();
            await Parallel.ForEachAsync(bestStoriesIds.Take(numberOfStories), cancellationToken, async (id, ct) =>
            {
                var item = await _hackerNewsApiClient.GetItemAsync(id, cancellationToken);
                if (item != null)
                {
                    var story = MapHackerNewsItemToStory(item);
                    bestStories.Add(story);
                }                  
            });

            var orderedBestStories = bestStories.OrderByDescending(x => x.Score).ToList();

            return new GetBestStoriesResponse(orderedBestStories);
        }

        private static Story MapHackerNewsItemToStory(HackerNewsItem item)
        {
            return new Story(
                Title: item.Title,
                Uri: !string.IsNullOrWhiteSpace(item.Url) ? new Uri(item.Url) : null,
                PostedBy: item.By,
                Time: item.Time.HasValue ? DateTimeOffset.FromUnixTimeSeconds(item.Time.Value).UtcDateTime : null,
                Score: item.Score,
                CommentCount: item.Descendants);
        }
    }
}
