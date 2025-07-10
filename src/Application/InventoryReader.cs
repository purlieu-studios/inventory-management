using Application.Mapping;
using Contract;
using Domain;
using Infrastructure;

namespace Application;

public class InventoryReader(IUnitOfWork unitOfWork) : IInventoryReader
{
    private readonly IGenericRepository<InventoryItemEntity> _repo = unitOfWork.Repository<InventoryItemEntity>();

    public async Task<InventoryItemModel?> GetInventoryItemAsync(Guid id)
    {
        var item = await _repo.FindAsync(i => i.Id == id);
        return item?.ToDomain();
    }

    public async Task<InventoryItemModel?> GetBySkuAsync(string sku)
    {
        var item = await _repo.FindAsync(i => i.SKU == sku);
        return item?.ToDomain();
    }

    public async Task<IReadOnlyList<InventoryItemModel>> GetByNameAsync(string name)
    {
        var items = await _repo.WhereAsync(i => i.Name == name);
        return [.. items.Select(i => i.ToDomain())];
    }

    public async Task<IReadOnlyList<InventoryItemModel>> GetByLocationAsync(string location)
    {
        var items = await _repo.WhereAsync(i => i.Location == location);
        return [.. items.Select(i => i.ToDomain())];
    }

    public async Task<IReadOnlyList<InventoryItemModel>> GetLowStockItemsAsync(int threshold)
    {
        var items = await _repo.WhereAsync(i => i.Quantity <= threshold);
        return [.. items.Select(i => i.ToDomain())];
    }

    public async Task<IReadOnlyList<InventoryItemModel>> GetActiveItemsAsync()
    {
        var items = await _repo.WhereAsync(i => i.IsActive);
        return [.. items.Select(i => i.ToDomain())];
    }

    public async Task<IReadOnlyList<InventoryItemModel>> GetInactiveItemsAsync()
    {
        var items = await _repo.WhereAsync(i => !i.IsActive);
        return [.. items.Select(i => i.ToDomain())];
    }

    public async Task<IReadOnlyList<InventoryItemModel>> GetItemsInPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var items = await _repo.WhereAsync(i => i.UnitPrice >= minPrice && i.UnitPrice <= maxPrice);
        return [.. items.Select(i => i.ToDomain())];
    }

    public async Task<IReadOnlyList<InventoryItemModel>> GetAllAsync()
    {
        var items = await _repo.WhereAsync();
        return [.. items.Select(i => i.ToDomain())];
    }
}
