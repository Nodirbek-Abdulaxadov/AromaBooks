using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using AromaBooks.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AromaBooks.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryInterface _categoryInterface;
    private readonly IBookInterface _bookInterface;
	private readonly ICommentInterface _commentInterface;
	private readonly UserManager<User> _userManager;
    private readonly ICartInterface _cartInterface;

    public HomeController(ICategoryInterface categoryInterface,
                           IBookInterface bookInterface,
                           ICommentInterface commentInterface,
                           UserManager<User> userManager,
                           ICartInterface cartInterface)
    {
        _categoryInterface = categoryInterface;
        _bookInterface = bookInterface;
		_commentInterface = commentInterface;
		_userManager = userManager;
        _cartInterface = cartInterface;
    }
    public async Task<IActionResult> Index()
    {
        HomeViewModel viewModel = new HomeViewModel()
        {
            TrendingBooks = await _bookInterface.Get4TrendingBooksAsync(),
            BestSellsBooks = await _bookInterface.Get10BestSellsBooksAsync(),
        };

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user != null)
        {
            var cartItems = await _cartInterface.GetCartsAsync(user.Id);
            ViewBag.count = cartItems.Count;
        }
        return View(viewModel);
    }

    public async Task<IActionResult> Books()
    {
        FilterModel filterModel = new();
        var list = await _bookInterface.FilterBookAsync(filterModel);
        var categories = await _categoryInterface.GetAllAsync();
        BooksFilterViewModel viewModel = new BooksFilterViewModel()
        {
            Books = list,
            FilterModel = filterModel,
            Categories = categories,
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Filter(BooksFilterViewModel viewModel)
    {
        var list = await _bookInterface.FilterBookAsync(viewModel.FilterModel);
        var categories = await _categoryInterface.GetAllAsync();
        viewModel = new BooksFilterViewModel()
        {
            Books = list,
            FilterModel = viewModel.FilterModel,
            Categories = categories,
        };
        return View("books", viewModel);
    }

    public async Task<IActionResult> BookDetails(int id)
    {
		var book = await _bookInterface.GetByIdWithCategoryAsync(id);
        var comments = await _commentInterface.GetBookCommentsWithRepliesAsync(id);
        var users = _userManager.Users.ToList();
        var user = await _userManager.GetUserAsync(HttpContext.User);
        comments = comments.Select(c => { c.User = users.FirstOrDefault(u => u.Id == c.UserId); return c; }).ToList();
        //fill comments replies user
        comments =  comments.Select(c =>
                                    {
                                        c.Replies = c.Replies.Select(r => { r.User = users.FirstOrDefault(u => u.Id == r.UserId); return r; }).ToList();
                                        return c;
                                    }).ToList();

        BookDetailViewModel viewModel = new BookDetailViewModel()
        {
			Book = book,
            Comments = comments,
            UserId = user?.Id ?? string.Empty,
		};
        if (user != null)
        {
            var cartItems = await _cartInterface.GetCartsAsync(user.Id);
            ViewBag.count = cartItems.Count;
        }
        return View(viewModel);
	}


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToCart(BookDetailViewModel viewModel)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _cartInterface.AddToCartAsync(user.Id, viewModel.BookId, viewModel.Quantity);
        return RedirectToAction("BookDetails", "Home", new { id = viewModel.BookId });
    }

    [Authorize]
    public async Task<IActionResult> Cart()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User); 
        var cartItems = await _cartInterface.GetCartsAsync(user.Id);
        ViewBag.count = cartItems.Count;
        return View(cartItems);
    }
}