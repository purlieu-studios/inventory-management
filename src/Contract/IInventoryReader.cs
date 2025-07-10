using Domain;

namespace Contract;
public interface IInventoryReader
{
    Task<InventoryItemModel?> GetInventoryItemAsync(Guid id);
    Task<InventoryItemModel?> GetBySkuAsync(string sku);
    Task<IReadOnlyList<InventoryItemModel>> GetByNameAsync(string name);
    Task<IReadOnlyList<InventoryItemModel>> GetByLocationAsync(string location);
    Task<IReadOnlyList<InventoryItemModel>> GetLowStockItemsAsync(int threshold);
    Task<IReadOnlyList<InventoryItemModel>> GetActiveItemsAsync();
    Task<IReadOnlyList<InventoryItemModel>> GetInactiveItemsAsync();
    Task<IReadOnlyList<InventoryItemModel>> GetItemsInPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<IReadOnlyList<InventoryItemModel>> GetAllAsync();
}