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
    private readonly SignInManager<User> _signInManager;

    public AuthController(UserManager<User> userManager,
                          IPasswordHasher<User> passwordHasher,
                          SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = _userManager.Users
                    .FirstOrDefault(u => u.UserName.EndsWith(viewModel.PhoneNumber.Replace("+", "")));
            if (user != null)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(user.UserName, 
                                         viewModel.Password, 
                                         viewModel.RememberMe,
                                         false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home", new { area = "" });
                }
            }
            ModelState.AddModelError("Password", "Incorrect phoneNumber or password!");
            return View();
        }

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

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("index", "home", new { area = "" });
    }
}