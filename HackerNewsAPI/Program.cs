using HackerNewsAPI.Endpoints;
using HackerNewsAPI.ExternalApi;
using HackerNewsAPI.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IHackerNewsApiClient, HackerNewsApiClient>();

builder.Services.AddSingleton<HackerNewsService>();
builder.Services.AddSingleton<IHackerNewsService>(
    x => new MemoryCachedHackerNewsService(x.GetRequiredService<HackerNewsService>(), x.GetRequiredService<IMemoryCache>()));

var app = builder.Build();

app.MapHackerNewsEndpoints();

app.Run();


// Make the implicit Program class public so test projects can access it
public partial class Program { }
