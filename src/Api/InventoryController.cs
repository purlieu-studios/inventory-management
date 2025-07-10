using Contract;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api;
#if !DEBUG
[Authorize(Roles = "Admin")]
#endif
[ApiController]
[Route("api/[controller]")]
public class InventoryController(IInventoryService inventoryService) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InventoryItemModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var item = await inventoryService.GetInventoryItemAsync(id);
        return item is not null ? Ok(item) : NotFound(new ProblemDetails { Title = "Not found" });
    }
}
