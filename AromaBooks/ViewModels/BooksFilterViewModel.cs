using AromaBooks.Data.Models;

namespace AromaBooks.ViewModels;

public class BooksFilterViewModel
{
    public List<Book> Books = new();
    public FilterModel FilterModel = new();
}
