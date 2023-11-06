using AromaBooks.Data.Enum;
using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AromaBooks.Data.Services;

public class BookService:IBookInterface
{
    private readonly AromaDbContext _dbContext;

    public BookService(AromaDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddAsync(Book book)
    {
        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var book = await GetByIdAsync(id);
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Book>> FilterBookAsync(FilterModel filterModel)
    {           
        IEnumerable<Book> books = new List<Book>();
        books = await _dbContext.Books.ToListAsync();
        if (filterModel.categoryId>0)
        {
            books = books.Where(b => b.CategoryId == filterModel.categoryId);
        }
        if (!string.IsNullOrEmpty(filterModel.auther))
        {
            books = books.Where(b => b.Author == filterModel.auther);
        }
        if (filterModel.minPrice>0)
        {
            books = books.Where(b => b.Price >= (decimal)filterModel.minPrice);
        }
        if (filterModel.maxPrice > 0)
        {
            books = books.Where(b => b.Price <= (decimal)filterModel.maxPrice);
        }

        if (!string.IsNullOrEmpty(filterModel.searchText))
        {
            books = books.Where(b => b.Title
                                      .ToLower()
                                        .Contains(filterModel.searchText
                                                               .ToLower()));
        }
        switch (filterModel.sortType)
        {
            case SortType.Unknown:
            case SortType.Title:
                {
                    if (filterModel.ascendingType == AscendingType.Ascending)
                    {
                        books = books.OrderBy(b => b.Title);
                    }
                    else
                    {
                        books = books.OrderByDescending(b => b.Title);
                    }
                }
                break;
            case SortType.Price:
                {
                    if (filterModel.ascendingType == AscendingType.Ascending)
                    {
                        books = books.OrderBy(b => b.Price);
                    }
                    else
                    {
                        books = books.OrderByDescending(b => b.Price);
                    }
                }
                break;
            case SortType.SellsCount:
                {
                    if (filterModel.ascendingType == AscendingType.Ascending)
                    {
                        books = books.OrderBy(b => b.sellsCount);
                    }
                    else
                    {
                        books = books.OrderByDescending(b => b.sellsCount);
                    }
                }
                break;
        }

        return books.Take(filterModel.count).ToList();

    }

    public async Task<List<Book>> Get10BestSellsBooksAsync()
    {
          return           await _dbContext.Books
                            .Include(b => b.Category)
                          .OrderByDescending(b => b.Id)
                          .Take(5)
                           .ToListAsync();
    }

    public async Task<List<Book>> Get4TrendingBooksAsync()
    {
               return     await _dbContext.Books
                          .Include(b => b.Category)
                          .OrderByDescending(b => b.Id)
                          .Take(5)
                           .ToListAsync();
    }

    public async Task<List<Book>> GetAllAsync()
        => await _dbContext.Books.ToListAsync();


    public async Task<Book> GetByIdAsync(int id)
        => await _dbContext.Books
            .FirstOrDefaultAsync(c => c.Id == id) ?? new Book();

    public async Task<Book> GetByIdWithCategoryAsync(int id)
        => await _dbContext.Books.Include(b => b.Category)
                    .FirstOrDefaultAsync(i => i.Id==id) ??
        new Book() { Title = "Enpty Book"} ;

    public async Task UpdateAsync(Book book)
    {
        _dbContext.Update(book);
        await _dbContext.SaveChangesAsync();
    }
}
