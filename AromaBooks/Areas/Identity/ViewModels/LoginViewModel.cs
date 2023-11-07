using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Areas.Identity.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}