# HackerNews API 

## How to run the application
Run following command from the terminal:

`dotnet run --project HackerNewsAPI/HackerNewsAPI.csproj`

You can trigger the API call by using your browser of choice by typing following in the address bar:

`http://localhost:5268/best-stories?numberOfItems=15`

## How to run the integration tests
Run following command from the terminal:

`dotnet test HackerNewsAPI.IntegrationTests/HackerNewsAPI.IntegrationTests.csproj`

## Assumptions
Application will be running only in one instance. The implemented approach for cache is not ideal for situation, where app is hosted on multiple servers. 

The HackerNews\' endpoint '/v0/beststories' returns IDs of stories already sorted by score in descending order. This assumption allows the service to limit number of requests (for getting item/story details) only to the _N "best stories"_, as requested in 'numberOfItems' parameter of implemented service (instead of getting details for all IDs returned by '/v0/beststories' endpoint).

## Potential enhancements or changes

### Cache improvement
The current approach for caching has following problem.
Let's assume, that the API receives (at the same time) requests with varying number of stories, like:

- `best-stories?numberOfStories=23`
- `best-stories?numberOfStories=100`
- `best-stories?numberOfStories=134`
- `best-stories?numberOfStories=400`

In such scenario, it is possible, that the service will make several calls to HackerNews API (depending on which request will be quicker with reaching the cache).

This case could be optimized, by making only one call to HackerNews API - for the highest number of stories requested. The subsequent requests would then reuse the cached response from the first request, without reaching HackerNews API.

### Other improvements
- Extensive logging.
- Add Swagger UI in development environment to ease the API browsing.
- Add API versioning.
- Distributed cache (which is more friendly with scenarions, where app is run on multiple instances).
