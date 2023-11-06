using AromaBooks.Data.Models;

namespace AromaBooks.Data.Interfaces;

public interface ICategoryInterface
{
   Task<List<Category> > GetAllAsync();

   Task<Category> GetByIdWithBookAsync(int id);

   Task<Category> GetByIdAsync(int id);

   Task AddAsync(Category category);

   Task UpdateAsync(Category category);

   Task DeleteAsync(int id);
    


}
