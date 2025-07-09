using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Infrastructure;

[Table("InventoryItems", Schema = "Warehouse")]
public class InventoryItemEntity : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(50)]
    public string SKU { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Required, MaxLength(100)]
    public string Location { get; set; } = default!;

    public bool IsActive { get; set; }
}
