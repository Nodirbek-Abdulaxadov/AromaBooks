using AromaBooks.Areas.Admin.Services;
using AromaBooks.Areas.Admin.ViewModels;
using AromaBooks.Data;
using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;


namespace AromaBooks.Areas.Admin.Controllers;

[Area("admin")]
public class BookController : Controller
{
    private readonly IBookInterface _books;
    private readonly ICategoryInterface _categories;

    private readonly IFileInterface _fileInterface;
    private readonly IToastNotification _toastNotification;

    public BookController(IBookInterface books,
                           ICategoryInterface categories,
                           IFileInterface fileInterface,
                           IToastNotification toastNotification)
    {
        _books = books;
        _categories = categories;
        _fileInterface = fileInterface;
        _toastNotification = toastNotification;
    }


    public async Task<IActionResult> Index()
    {

        var list = await _books.GetAllAsync();
        return View(list);
    }
    public async Task<IActionResult> View(int id)
    {
        var book = await _books.GetByIdWithCategoryAsync(id);
        return View(book);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {

        var list = await _categories.GetAllAsync();
        AddBookViewModel viewModel = new AddBookViewModel()
        {
            Categories = list,
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBookViewModel viewModel)
    {
        if(ModelState.IsValid)
        {
            string imageURL = _fileInterface.Save(viewModel.FileName);

            Book newBook = new()
            {
                Title = viewModel.Title,
                Author = viewModel.Author,
                Price = viewModel.Price,
                Description = viewModel.Description,
                PageCount = viewModel.PageCount,
                PublishedYear = viewModel.PublishedYear,
                CategoryId = viewModel.CategoryId,
                ImageUrl = imageURL,
                Category = null
            };

            await _books.AddAsync(newBook);

            return RedirectToAction("Index");
        }
		var list = await _categories.GetAllAsync();
        viewModel.Categories = list;
		return View(viewModel);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var book = await _books.GetByIdAsync(id);
        if(book.Id== 0)
        {
            _toastNotification.AddErrorToastMessage("Book not found");
            return RedirectToAction("Index");
        }
       string img = book.ImageUrl;
       await _books.DeleteAsync(id);
	   _fileInterface.Delete(img);
	    _toastNotification.AddSuccessToastMessage("Seccesfully deleted ");
       return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {

        var book = await _books.GetByIdAsync(id);
        if(book.Id == 0)
        {
            _toastNotification.AddErrorToastMessage("Book not found");
            return RedirectToAction("Index");
        }

        var categories = await _categories.GetAllAsync();
        EditBookViewModel viewModel = new()
        {
            Id = book.Id,
            Categories = categories,
            Title = book.Title,
            Price = book.Price,
            Description = book.Description,
            Author = book.Author,
            CategoryId = book.CategoryId,
            ImageUrl = book.ImageUrl,
            PageCount = book.PageCount,
            PublishedYear = book.PublishedYear,
        };


        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBookViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            string image = viewModel.ImageUrl;
            if (viewModel.File != null)
            {
                _fileInterface.Delete(image);
                image = _fileInterface.Save(viewModel.File);
                Book book = new()
                {
                    Id = viewModel.Id,
                    Title = viewModel.Title,
                    Author = viewModel.Author,
                    Price = viewModel.Price,
                    Description = viewModel.Description,
                    PageCount = viewModel.PageCount,
                    PublishedYear = viewModel.PublishedYear,
                    CategoryId = viewModel.CategoryId,
                    ImageUrl = image,
                    Category = null
                };
                await _books.UpdateAsync(book);
            }
            _toastNotification.AddSuccessToastMessage("Seccessfully update");
            return RedirectToAction("index");
        }

        viewModel.Categories = await _categories.GetAllAsync();
        _toastNotification.AddErrorToastMessage("Fill out all required fields!");
        return View(viewModel);

    }


}
