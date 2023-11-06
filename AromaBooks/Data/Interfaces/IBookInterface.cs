using AromaBooks.Data.Enum;
using AromaBooks.Data.Models;

namespace AromaBooks.Data.Interfaces
{
    public interface IBookInterface
    {
        Task<List<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<Book> GetByIdWithCategoryAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);

        Task<List<Book>> Get4TrendingBooksAsync();

        Task<List<Book>> Get10BestSellsBooksAsync();

        Task<List<Book>> FilterBookAsync(FilterModel model );
    }
}
