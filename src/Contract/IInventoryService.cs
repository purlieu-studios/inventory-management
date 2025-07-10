using Domain;

namespace Contract;

public interface IInventoryService
{
    Task<InventoryItemModel?> GetInventoryItemAsync(Guid id);
}
