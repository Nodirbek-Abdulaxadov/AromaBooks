using AromaBooks.Data.Enum;

namespace AromaBooks.Data.Models;

public record FilterModel
(
    int categoryId = 0,
   string? auther = null,
   double minPrice =0,
   double maxPrice = 0,
   string? searchText = null,
   SortType sortType = SortType.Unknown,
   AscendingType ascendingType = AscendingType.Ascending,
   int count = 9
);
