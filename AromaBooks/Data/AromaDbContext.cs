using AromaBooks.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AromaBooks.Data;

public class AromaDbContext:DbContext
{
    public AromaDbContext(DbContextOptions<AromaDbContext>options)
        : base(options){ }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Books)
            .WithOne(b => b.Category)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
