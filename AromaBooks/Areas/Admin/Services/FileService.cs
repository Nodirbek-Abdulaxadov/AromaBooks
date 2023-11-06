using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AromaBooks.Areas.Admin.Services;

public class FileService : IFileInterface
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public void Delete(string fileName)
    {
        if (fileName is not null)
        {
            string uplodFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            string filePath = Path.Combine(uplodFolder, fileName);
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }

    public string Save(IFormFile file)
    {
        string uniqueName = string.Empty;
        if (file != null)
        {
            string uplodFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            uniqueName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uplodFolder, uniqueName);
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Close();
        }

        return uniqueName;
    }
}