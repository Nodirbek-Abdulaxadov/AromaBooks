using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Data.Models;

public class Category :BaseModel
{
    [Required(ErrorMessage = "Category name must contains at least 1 character")]
    [StringLength(100, ErrorMessage = "Category name must contains less than 100 character")]
    public string Name { get; set; } = string.Empty;

    public List<Book> Books = new();
}
