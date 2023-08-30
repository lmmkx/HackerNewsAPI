using HackerNewsAPI.Services;

namespace HackerNewsAPI.Endpoints
{
    public static class HackerNewsEndpoints
    {
        public static void MapHackerNewsEndpoints(this WebApplication app)
        {
            app.MapGet("/best-stories", GetBestStoriesAsync);
        }

        internal static async Task<IResult> GetBestStoriesAsync(
            ushort numberOfStories, 
            IHackerNewsService hackerNewsService,
            CancellationToken cancellationToken)
        {

            var bestStories = await hackerNewsService.GetBestStoriesAsync(numberOfStories, cancellationToken);

            if (bestStories.IsError)
                return Results.BadRequest();
            else
                return Results.Ok(bestStories.Items);
        }
    }
}
