using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace AromaBooks.Areas.Admin.Controllers;

[Area("admin")]
public class CategoryController : Controller
{
    private readonly ICategoryInterface _categories;

    public IToastNotification _toastNotification;

    public CategoryController(ICategoryInterface categories,
                              IToastNotification toastNotification)
    {
        _categories = categories;
        _toastNotification = toastNotification;
    }


    public async Task<IActionResult> Index()
    {
        var categoryList = await _categories.GetAllAsync();
        return View(categoryList);
    }
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Category category)
    {
        if (ModelState.IsValid)
        {
            await _categories.AddAsync(category);
            return RedirectToAction("index");
        }

        return View();
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categories.GetByIdAsync(id);
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            await _categories.UpdateAsync(category);
            return RedirectToAction("index");
        }
        return View("endit", category);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var categoryWithBook = await _categories.GetByIdWithBookAsync(id);

        if (categoryWithBook.Id == 0)
        {
            _toastNotification.AddErrorToastMessage("Category Not Found!");
            return RedirectToAction("index");
        }

        if (categoryWithBook.Books.Any())
        {
            _toastNotification.AddErrorToastMessage("Category can't be deleted!");
            return RedirectToAction("index");
        }

        await _categories.DeleteAsync(id);
        _toastNotification.AddSuccessToastMessage("Successfully deleted!");
        return RedirectToAction("index");
    }

}
