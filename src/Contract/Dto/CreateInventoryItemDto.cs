using System.ComponentModel.DataAnnotations;

namespace Contract;
public class CreateInventoryItemDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(50)]
    public string SKU { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Required, MaxLength(100)]
    public string Location { get; set; } = default!;
}

