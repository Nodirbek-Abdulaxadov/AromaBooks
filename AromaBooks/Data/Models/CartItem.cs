namespace AromaBooks.Data.Models;

public class CartItem : BaseModel
{
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int BookId { get; set; }
    public Book Book = new();
    public string UserId { get; set; } = string.Empty;
}