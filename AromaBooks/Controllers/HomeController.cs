using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using AromaBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AromaBooks.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryInterface _categoryInterface;
    private readonly IBookInterface _bookInterface;

    public HomeController(ICategoryInterface categoryInterface,
                           IBookInterface bookInterface)
    {
        _categoryInterface = categoryInterface;
        _bookInterface = bookInterface;
    }
    public async Task<IActionResult> Index()
    {
        HomeViewModel viewModel = new HomeViewModel()
        {
            TrendingBooks = await _bookInterface.Get4TrendingBooksAsync(),
            BestSellsBooks = await _bookInterface.Get10BestSellsBooksAsync(),
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Books()
    {
        FilterModel filterModel = new();
        var list = await _bookInterface.FilterBookAsync(filterModel);
        BooksFilterViewModel viewModel = new BooksFilterViewModel()
        {
            Books = list,
            FilterModel = filterModel

        };
        return View(viewModel);
    }
}