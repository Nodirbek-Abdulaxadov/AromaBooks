using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using AromaBooks.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AromaBooks.Controllers;

public class CommentController : Controller
{
    private readonly ICommentInterface _commentInterface;
    private readonly UserManager<User> _userManager;

    public CommentController(ICommentInterface commentInterface,
                             UserManager<User> userManager)
    {
        _commentInterface = commentInterface;
        _userManager = userManager;
    }

    [Authorize]
    public async Task<IActionResult> Add(BookDetailViewModel viewModel)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user != null)
        {
            var comment = new Comment()
            {
                Content = viewModel.Comment,
                BookId = viewModel.BookId,
                CommentedTime = DateTime.Now,
                UserId = user.Id
            };

            if (viewModel.IsReply)
            {
               comment.CommentId = viewModel.CommentId;
                comment.IsReply = true;
            }
            
            
            await _commentInterface.AddCommentAsync(comment);
        }

        return RedirectToAction("BookDetails", "Home", new { id = viewModel.BookId });
    }

    public async Task<IActionResult> Delete(int id, int bookId)
    {
        var comment = await _commentInterface.GetCommentAsync(id);
        if (comment != null)
        {
            await _commentInterface.DeleteCommentAsync(id);
        }
        return RedirectToAction("BookDetails", "Home", new { id = bookId });
    }
}
