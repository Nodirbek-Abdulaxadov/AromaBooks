using AromaBooks.Data.Models;

namespace AromaBooks.ViewModels;

public class BookDetailViewModel
{
    public Book Book = new();
    public List<Comment> Comments = new();

    public string Comment { get; set; } = string.Empty;
    public int BookId { get; set; }
    public bool IsReply { get; set; }
    public int CommentId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int count { get; set; }
    public int Quantity { get; set; }
}
