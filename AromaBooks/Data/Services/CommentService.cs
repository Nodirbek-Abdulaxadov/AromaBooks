using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AromaBooks.Data.Services;

public class CommentService : ICommentInterface
{
    private readonly AromaDbContext _dbContext;
    public CommentService(AromaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var comment = _dbContext.Comments.FirstOrDefault(x => x.Id == id);
        if (comment != null)
        {
            _dbContext.Comments.Remove(comment);
            var replies = await _dbContext.Comments.Where(x => x.CommentId == id).ToListAsync();
            foreach (var reply in replies)
            {
                _dbContext.Comments.Remove(reply);
                await _dbContext.SaveChangesAsync();
            }
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Comment> GetCommentAsync(int id)
        => await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Comment>> GetCommentsAsync()
        => await _dbContext.Comments.ToListAsync();

    public async Task<List<Comment>> GetBookCommentsWithRepliesAsync(int bookId)
    {
        var list = await _dbContext.Comments.Where(x => x.BookId == bookId && 
                                                        x.IsReply == false)
                                            .OrderByDescending(c => c.CommentedTime)                                
                                            .ToListAsync();
        //fill replies
        list = list.Select(x =>
        {
			x.Replies = _dbContext.Comments.Where(y => y.CommentId == x.Id)
                                           .OrderByDescending(c => c.CommentedTime)
                                           .ToList();
			return x;
		}).ToList();

        return list;
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync();
    }
}
