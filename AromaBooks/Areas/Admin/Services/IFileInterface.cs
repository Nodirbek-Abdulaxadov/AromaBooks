namespace AromaBooks.Areas.Admin.Services;

// Save and Delete
public interface IFileInterface
{
    string Save(IFormFile file);
    void Delete(string fileName);
}
