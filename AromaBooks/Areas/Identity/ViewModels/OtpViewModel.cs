using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Areas.Identity.ViewModels;

public class OtpViewModel
{
    [Required]
    public int Code { get; set; }

    public string CodeHash { get; set; } = string.Empty;
}