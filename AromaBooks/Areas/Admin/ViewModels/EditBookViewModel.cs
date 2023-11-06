using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Areas.Admin.ViewModels;

public class EditBookViewModel:AddBookViewModel
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

	public IFormFile? File { get; set; }
}
