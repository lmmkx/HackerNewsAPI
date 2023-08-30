namespace HackerNewsAPI.ExternalApi
{
    public record HackerNewsItem(
        string Title, 
        string Url, 
        string By, 
        int? Time, 
        int? Score, 
        int? Descendants);
}
