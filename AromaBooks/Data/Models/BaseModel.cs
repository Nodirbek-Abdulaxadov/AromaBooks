using System.ComponentModel.DataAnnotations;

namespace AromaBooks.Data.Models;

public  abstract class BaseModel
{
    [Key , Required]
    public int Id { get; set; }
}
