using Contract;
using Domain;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api;
#if !DEBUG
[Authorize(Roles = "Admin")]
#endif
[ApiController]
[Route("api/[controller]")]
public class InventoryController(
    IInventoryReader reader,
    IInventoryWriter writer
) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InventoryItemModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await reader.GetInventoryItemAsync(id);
        return item is not null
            ? Ok(item)
            : NotFound(new ProblemDetails { Title = "Inventory item not found." });
    }

    [HttpGet("sku/{sku}")]
    [ProducesResponseType(typeof(InventoryItemModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySku(string sku)
    {
        var item = await reader.GetBySkuAsync(sku);
        return item is not null
            ? Ok(item)
            : NotFound(new ProblemDetails { Title = "Inventory item not found." });
    }

    [HttpGet("name/{name}")]
    public async Task<IReadOnlyList<InventoryItemModel>> GetByName(string name)
        => await reader.GetByNameAsync(name);

    [HttpGet("location/{location}")]
    public async Task<IReadOnlyList<InventoryItemModel>> GetByLocation(string location)
        => await reader.GetByLocationAsync(location);

    [HttpGet("low-stock")]
    public async Task<IReadOnlyList<InventoryItemModel>> GetLowStock([FromQuery] int threshold = 10)
        => await reader.GetLowStockItemsAsync(threshold);

    [HttpGet("active")]
    public async Task<IReadOnlyList<InventoryItemModel>> GetActive()
        => await reader.GetActiveItemsAsync();

    [HttpGet("inactive")]
    public async Task<IReadOnlyList<InventoryItemModel>> GetInactive()
        => await reader.GetInactiveItemsAsync();

    [HttpGet("price-range")]
    public async Task<IReadOnlyList<InventoryItemModel>> GetByPriceRange(
        [FromQuery] decimal min,
        [FromQuery] decimal max)
        => await reader.GetItemsInPriceRangeAsync(min, max);

    [HttpGet]
    public async Task<IReadOnlyList<InventoryItemModel>> GetAll()
        => await reader.GetAllAsync();

    [HttpPost]
    [ProducesResponseType(typeof(InventoryItemModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateInventoryItemDto dto)
    {
        var result = await writer.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, UpdateInventoryItemDto dto)
    {
        try
        {
            await writer.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new ProblemDetails { Title = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await writer.DeleteAsync(id);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new ProblemDetails { Title = ex.Message });
        }
    }

    [HttpPost("{id:guid}/adjust-quantity")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdjustQuantity(Guid id, [FromBody] int delta)
    {
        try
        {
            await writer.AdjustQuantityAsync(id, delta);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new ProblemDetails { Title = ex.Message });
        }
    }
}