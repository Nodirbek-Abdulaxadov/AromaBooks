using Microsoft.AspNetCore.Mvc;

namespace AromaBooks.Areas.Identity.Controllers;

[Area("Identity")]
public class AuthController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
}