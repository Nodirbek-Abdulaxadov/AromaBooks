using AromaBooks.Areas.Identity.ViewModels;
using AromaBooks.Data.Models;
using Messager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AromaBooks.Areas.Identity.Controllers;

[Area("Identity")]
public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(UserManager<User> userManager,
                          IPasswordHasher<User> passwordHasher)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            User user = new()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Address = viewModel.Address,
                PhoneNumber = viewModel.PhoneNumber
            };

            string userName = $"{viewModel.FirstName}{viewModel.PhoneNumber.Replace("+","")}";

            await _userManager.SetUserNameAsync(user, userName);
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("otp", "auth", user);
            }

            return View();
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Otp(User user)
    {
        using var messager = new Message();
        var res = await messager.SendSMSAsync(user.PhoneNumber);
        var viewModel = new OtpViewModel()
        {
            CodeHash = (res.Code + 12345).ToString()
        };

        return View(viewModel);
    }
}