using Bogus;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Seeding;

public static partial class SeedExtensions
{
    public static async Task SeedInventoryAsync(MainDbContext context)
    {
        if (await context.InventoryItems.AnyAsync()) return;

        var faker = new Faker<InventoryItemEntity>()
            .RuleFor(i => i.Id, _ => Guid.NewGuid())
            .RuleFor(i => i.Name, f => f.Commerce.ProductName())
            .RuleFor(i => i.SKU, f => f.Commerce.Ean8())
            .RuleFor(i => i.Description, f => f.Commerce.ProductDescription())
            .RuleFor(i => i.Quantity, f => f.Random.Int(0, 1000))
            .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 500))
            .RuleFor(i => i.Location, f => f.Address.City())
            .RuleFor(i => i.IsActive, f => f.Random.Bool(0.9f))
            .RuleFor(i => i.InsertedAt, f => f.Date.PastOffset(1))
            .RuleFor(i => i.UpdatedAt, (f, i) => i.InsertedAt.AddDays(f.Random.Int(1, 100)));

        var items = faker.Generate(100);
        await context.InventoryItems.AddRangeAsync(items);
    }

}
