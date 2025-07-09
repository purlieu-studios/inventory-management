using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options) { }
}