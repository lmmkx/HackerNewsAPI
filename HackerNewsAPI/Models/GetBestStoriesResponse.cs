namespace HackerNewsAPI.Models
{
    public class GetBestStoriesResponse
    {
        public IReadOnlyCollection<Story> Items { get; private set; }
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        public string? ErrorMessage { get; private set; }

        public GetBestStoriesResponse(IEnumerable<Story> items)
        {
            Items = items as IReadOnlyCollection<Story> ?? items.ToList();
        }

        public GetBestStoriesResponse(string errorMessage)
        {
            Items = new List<Story>();
            ErrorMessage = errorMessage;
        }
    } 
}
