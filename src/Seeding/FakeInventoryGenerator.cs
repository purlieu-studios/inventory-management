using Bogus;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Seeding;

public static partial class SeedExtensions
{
    public static async Task SeedInventoryAsync(MainDbContext context)
    {
        if (await context.InventoryItems.AnyAsync()) return;

        var itemTypes = new[]
        {
        new { Type = "Arcane Focus", BasePrice = 1500m, MinQty = 1, MaxQty = 5 },
        new { Type = "Potion", BasePrice = 250m, MinQty = 10, MaxQty = 40 },
        new { Type = "Scroll", BasePrice = 100m, MinQty = 20, MaxQty = 80 },
        new { Type = "Weapon", BasePrice = 800m, MinQty = 2, MaxQty = 15 },
        new { Type = "Armor", BasePrice = 1200m, MinQty = 3, MaxQty = 10 },
        new { Type = "Ingredient", BasePrice = 25m, MinQty = 50, MaxQty = 200 }
    };

        var locations = new[]
        {
        "Vault of Flame",
        "Elven Armory",
        "Alchemist's Lab",
        "Scroll Repository",
        "Shadow Market",
        "Druid's Grove"
    };

        var prefixes = new[] { "Enchanted", "Cursed", "Ancient", "Mystic", "Ethereal", "Runed", "Shimmering", "Obsidian", "Gleaming", "Forgotten" };
        var suffixes = new[] { "Warding", "Fury", "Wisdom", "Decay", "Blight", "Grace", "Torment", "Echoes", "Fire", "Nightfall" };
        var materials = new[] { "iron", "silver", "mithril", "oakwood", "drakeskin", "voidstone", "emberglass" };
        var rarities = new[] { "common", "uncommon", "rare", "epic", "legendary" };

        var random = new Random();

        var items = itemTypes.SelectMany(type =>
        {
            var faker = new Faker<InventoryItemEntity>()
                .RuleFor(i => i.Id, _ => Guid.NewGuid())
                .RuleFor(i => i.Name, f =>
                {
                    var prefix = f.PickRandom(prefixes);
                    var suffix = f.PickRandom(suffixes);
                    return $"{prefix} {type.Type} of {suffix}";
                })
                .RuleFor(i => i.SKU, f => $"FN-{type.Type[..2].ToUpperInvariant()}-{f.Random.Number(1000, 9999)}")
               .RuleFor(i => i.Description, (f, i) =>
               {
                   var origin = f.PickRandom(
                       "the forges beneath Mount Vaelgar",
                       "the lost vaults of the Stormforged Kings",
                       "the lunar sanctum of the Astral Magi",
                       "the bloodwood groves of Eldara",
                       "the obsidian towers of the Whispering Guild",
                       "the battlefields of the Everwar",
                       "the shattered halls of the Crystal Dominion"
                   );

                   var bearer = f.PickRandom(
                       "renegade archmages",
                       "wandering sellswords",
                       "druidic high councils",
                       "forgotten cults of the Underdeep",
                       "elite guards of the Sapphire Throne",
                       "healers from the northern tribes",
                       "necromancers who survived the Cataclysm"
                   );

                   var enchantment = f.PickRandom(
                       "whispers to its wielder in dreams",
                       "glows faintly when danger nears",
                       "is warm to the touch, even in blizzards",
                       "resonates with the heartbeat of its master",
                       "sings when unsheathed at dusk",
                       "rejects those it deems unworthy",
                       "contains the soul of a slain sorcerer"
                   );

                   var loreTag = f.PickRandom(
                       "Legends claim it played a role in the Siege of Emberdeep.",
                       "Only five of its kind were ever forged.",
                       "The Church of Vire banned its use due to 'unnatural effects.'",
                       "Collectors from the Shadow Empire will pay handsomely for it.",
                       "It is said to defy time, never aging nor rusting.",
                       "An old prophecy speaks of its return during the Eclipse Cycle."
                   );

                   return $"Forged in {origin}, this {i.Name.ToLower()} was once wielded by {bearer}. It {enchantment}. {loreTag}";
               })
                .RuleFor(i => i.Quantity, _ => random.Next(type.MinQty, type.MaxQty + 1))
                .RuleFor(i => i.UnitPrice, _ => Math.Round(type.BasePrice * (decimal)(0.85 + random.NextDouble() * 0.4), 2))
                .RuleFor(i => i.Location, f => f.PickRandom(locations))
                .RuleFor(i => i.IsActive, _ => true)
                .RuleFor(i => i.InsertedAt, f => f.Date.PastOffset(1))
                .RuleFor(i => i.UpdatedAt, (f, i) => i.InsertedAt.AddDays(random.Next(1, 45)));

            return faker.Generate(10); // 10 per type
        }).ToList();

        await context.InventoryItems.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

}
