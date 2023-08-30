namespace HackerNewsAPI.Models
{
    public record Story(
        string Title, 
        Uri? Uri, 
        string PostedBy, 
        DateTime? Time, 
        int? Score, 
        int? CommentCount);
    
}
