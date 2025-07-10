using Application.Mapping;
using Contract;
using Domain;
using Domain.Exceptions;
using Infrastructure;

namespace Application;

public class InventoryWriter(IUnitOfWork unitOfWork) : IInventoryWriter
{
    public async Task<InventoryItemModel> CreateAsync(CreateInventoryItemDto dto)
    {
        var repo = unitOfWork.Repository<InventoryItemEntity>();

        var entity = new InventoryItemEntity
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SKU = dto.SKU,
            Description = dto.Description,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            Location = dto.Location,
            IsActive = true,
            InsertedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await repo.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();

        return entity.ToDomain();
    }

    public async Task UpdateAsync(Guid id, UpdateInventoryItemDto dto)
    {
        var repo = unitOfWork.Repository<InventoryItemEntity>();

        var entity = await repo.FindAsync(e => e.Id == id)
                     ?? throw new EntityNotFoundException("Inventory Item", id);

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Quantity = dto.Quantity;
        entity.UnitPrice = dto.UnitPrice;
        entity.Location = dto.Location;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var repo = unitOfWork.Repository<InventoryItemEntity>();
        var entity = await repo.FindAsync(e => e.Id == id)
                     ?? throw new EntityNotFoundException("Inventory Item", id);

        await repo.DeleteAsync(entity);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task AdjustQuantityAsync(Guid id, int delta)
    {
        var repo = unitOfWork.Repository<InventoryItemEntity>();
        var entity = await repo.FindAsync(e => e.Id == id)
                     ?? throw new EntityNotFoundException("Inventory Item", id);

        entity.Quantity += delta;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await unitOfWork.SaveChangesAsync();
    }
}
