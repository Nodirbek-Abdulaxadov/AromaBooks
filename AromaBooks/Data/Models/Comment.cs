namespace AromaBooks.Data.Models;

public class Comment : BaseModel
{
    public string UserId { get; set; } = string.Empty;
    public User User = new();
    public DateTime CommentedTime { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsReply { get; set; }
    public int CommentId { get; set; }
    public int BookId { get; set; }

    public List<Comment> Replies = new();
}