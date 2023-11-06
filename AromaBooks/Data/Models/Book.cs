using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Data.Models;

public class Book:BaseModel 
{
    [Required, StringLength(100)]

    public string Title { get; set; } = string.Empty;
       
    [StringLength(200)]

    public string Description { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    public int PageCount { get; set; }

    [Required]
    public int PublishedYear { get; set; }

    public string ImageUrl { get; set; }= string.Empty;
    public int sellsCount { get; set; }   

    [Required]
    public int CategoryId { get; set; }
    public Category Category = new Category();



  

}
