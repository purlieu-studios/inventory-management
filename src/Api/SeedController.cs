using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seeding;

namespace Api;
#if !DEBUG
[Authorize(Roles = "Admin")]
#endif
[ApiController]
[Route("api/[controller]")]
public class SeedController(ISeedService seedService) : ControllerBase
{
    [HttpPost("seed")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> SeedData()
    {
        await seedService.SeedDevelopmentDataAsync();
        return Ok("Seeding completed.");
    }
}