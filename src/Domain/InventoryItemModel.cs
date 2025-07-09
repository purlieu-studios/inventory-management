namespace Domain;
public record InventoryItemModel(
    Guid Id,
    string Name,
    string SKU,
    string? Description,
    int Quantity,
    decimal UnitPrice,
    string Location,
    bool IsActive,
    DateTimeOffset InsertedAt,
    DateTimeOffset UpdatedAt
);