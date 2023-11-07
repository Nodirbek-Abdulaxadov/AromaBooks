using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Areas.Identity.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [MinLength(8), MaxLength(15)]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
}
