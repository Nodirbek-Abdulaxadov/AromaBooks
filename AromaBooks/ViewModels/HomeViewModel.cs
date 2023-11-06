using AromaBooks.Data.Models;

namespace AromaBooks.ViewModels;

public class HomeViewModel
{
    

    public List<Book> TrendingBooks = new();

    public List<Book> BestSellsBooks = new();
}
