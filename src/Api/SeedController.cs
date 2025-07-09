using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seeding;

namespace Api;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InventoryController(ISeedService seedService) : ControllerBase
{
    [HttpPost("seed")]
#if !DEBUG
[Authorize(Roles = "Admin")]
#endif
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SeedData()
    {
        await seedService.SeedDevelopmentDataAsync();
        return Ok("Seeding completed.");
    }
}