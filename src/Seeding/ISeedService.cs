namespace Seeding;

public interface ISeedService
{
    Task SeedDevelopmentDataAsync(int seed = 1000);
}
