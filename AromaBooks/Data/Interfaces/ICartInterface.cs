using AromaBooks.Data.Models;

namespace AromaBooks.Data.Interfaces;

public interface ICartInterface
{
    Task<List<CartItem>> GetCartAsync(string userId);
    Task AddToCartAsync(string userId, int bookId, int quantity);
    Task RemoveFromCartAsync(string userId, int bookId);
    Task ClearCartAsync(string userId);
    Task<int> GetTotalPriceAsync(string userId);
    Task<int> GetTotalQuantityAsync(string userId);
    Task<List<CartItem>> GetCartsAsync(string userId);
    Task<CartItem> GetCartAsync(string userId, int bookId);
    Task UpdateCartAsync(string userId, int bookId, int quantity);
}