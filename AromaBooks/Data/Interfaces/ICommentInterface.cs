using AromaBooks.Data.Models;

namespace AromaBooks.Data.Interfaces;

public interface ICommentInterface
{
    Task<Comment> GetCommentAsync(int id);
    Task<List<Comment>> GetCommentsAsync();
    Task<List<Comment>> GetBookCommentsWithRepliesAsync(int bookId);
    Task AddCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
}