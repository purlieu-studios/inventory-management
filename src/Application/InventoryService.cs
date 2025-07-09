using Contract;
using Infrastructure;

namespace Application;

public class InventoryService(IUnitOfWork unitOfWork) : IInventoryService
{
}
