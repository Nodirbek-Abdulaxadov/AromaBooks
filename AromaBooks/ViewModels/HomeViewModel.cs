using AromaBooks.Data.Models;

namespace AromaBooks.ViewModels;

public class HomeViewModel
{
    public int count = 0;

    public List<Book> TrendingBooks = new();

    public List<Book> BestSellsBooks = new();
}
