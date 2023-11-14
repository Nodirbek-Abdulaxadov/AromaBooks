namespace AromaBooks.Data.Models;

public class Review : BaseModel
{
    public string UserId { get; set; } = string.Empty;
    public User User = new();
    public int BookId { get; set; }
    public DateTime ReviewedTime { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
}