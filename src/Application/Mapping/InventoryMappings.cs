using Domain;
using Infrastructure;

namespace Application.Mapping;
public static class InventoryMappings
{
    public static InventoryItemModel ToDomain(this InventoryItemEntity entity) => new(
        entity.Id,
        entity.Name,
        entity.SKU,
        entity.Description,
        entity.Quantity,
        entity.UnitPrice,
        entity.Location,
        entity.IsActive,
        entity.InsertedAt,
        entity.UpdatedAt
    );

    public static InventoryItemEntity ToEntity(this InventoryItemModel model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        SKU = model.SKU,
        Description = model.Description,
        Quantity = model.Quantity,
        UnitPrice = model.UnitPrice,
        Location = model.Location,
        IsActive = model.IsActive,
        InsertedAt = model.InsertedAt,
        UpdatedAt = model.UpdatedAt
    };
}