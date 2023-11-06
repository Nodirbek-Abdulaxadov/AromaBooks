using AromaBooks.Data.Interfaces;
using AromaBooks.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AromaBooks.Data.Services;

public class CategoryServices : ICategoryInterface
{
    private readonly AromaDbContext _dbContext;

    public CategoryServices(AromaDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }


    public async Task<List<Category>> GetAllAsync()
        => await _dbContext.Categories.ToListAsync();



    public async Task<Category> GetByIdAsync(int id)
        => await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id) ?? new Category();

    public async Task<Category> GetByIdWithBookAsync(int id)
            =>await _dbContext.Categories
                        .Include(c => c.Books)
                        .FirstOrDefaultAsync(c => c.Id==id)
                        ?? new Category() { Name = "Empty category"};
      
    

    public async Task UpdateAsync(Category category)
    {
        _dbContext.Update(category);
        await _dbContext.SaveChangesAsync();
    }
}