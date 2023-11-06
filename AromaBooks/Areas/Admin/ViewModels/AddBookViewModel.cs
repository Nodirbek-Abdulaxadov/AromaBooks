using AromaBooks.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Areas.Admin.ViewModels;

public class AddBookViewModel
{

    [Required, StringLength(100)]

    public string Title { get; set; } = string.Empty;

    [StringLength(200)]

    public string Description { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int PageCount { get; set; }

    [Required]
    public int PublishedYear { get; set; }

   
    public IFormFile? FileName { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public List<Category> Categories = new();

}
