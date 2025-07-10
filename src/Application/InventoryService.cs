using Application.Mapping;
using Contract;
using Domain;
using Infrastructure;

namespace Application;

public class InventoryService(IUnitOfWork unitOfWork) : IInventoryService
{

    public async Task<InventoryItemModel?> GetInventoryItemAsync(Guid id)
    {
        var repo = unitOfWork.Repository<InventoryItemEntity>();

        var item = await repo.FindAsync(i => i.Id == id);

        return item?.ToDomain();
    }
}
