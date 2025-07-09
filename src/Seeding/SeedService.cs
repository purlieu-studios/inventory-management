using Bogus;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Seeding;


public class SeedService : ISeedService
{
    private readonly MainDbContext _context;

    public SeedService(MainDbContext context)
    {
        _context = context;
    }

    public async Task SeedDevelopmentDataAsync(int seed = 1337)
    {
        if (await _context.InventoryItems.AnyAsync())
            return;

        Randomizer.Seed = new Random(seed);

        await SeedExtensions.SeedInventoryAsync(_context);
    }
}
