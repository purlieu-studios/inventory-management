using Domain;

namespace Contract;

public interface IInventoryWriter
{
    Task<InventoryItemModel> CreateAsync(CreateInventoryItemDto dto);
    Task UpdateAsync(Guid id, UpdateInventoryItemDto dto);
    Task DeleteAsync(Guid id);
    Task AdjustQuantityAsync(Guid id, int delta);
}
