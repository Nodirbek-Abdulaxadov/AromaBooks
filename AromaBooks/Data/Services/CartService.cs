using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AromaBooks.Data.Services;

public class CartService : ICartInterface
{
    private readonly AromaDbContext _dbContext;

    public CartService(AromaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// add cart to database
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="bookId"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public async Task AddToCartAsync(string userId, int bookId, int quantity)
    {
        _dbContext.CartItems.Add(new CartItem()
        {
            BookId = bookId,
            Quantity = quantity,
            UserId = userId
        });

        await _dbContext.SaveChangesAsync();
    }

    public Task ClearCartAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CartItem>> GetCartsAsync(string userId)
    {
        var userCartItems = await _dbContext.CartItems
                                            .Where(c => c.UserId == userId)
                                            .ToListAsync();
        var Books = await _dbContext.Books.ToListAsync();
        foreach (var item in userCartItems)
        {
            var book = Books.FirstOrDefault(b => b.Id == item.BookId);
            item.Book = book ?? new Book();
            item.TotalPrice = item.Book.Price * item.Quantity;
        }

        return userCartItems;
    }

    public Task<CartItem> GetCartAsync(string userId, int bookId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CartItem>> GetCartAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalPriceAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalQuantityAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFromCartAsync(string userId, int bookId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCartAsync(string userId, int bookId, int quantity)
    {
        throw new NotImplementedException();
    }
}
