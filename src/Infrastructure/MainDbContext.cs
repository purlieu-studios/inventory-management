using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MainDbContext : DbContext
{
    public DbSet<InventoryItemEntity> InventoryItems { get; set; }
    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options) { }
}