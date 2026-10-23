using HRIS.API.Data.Repositories;
using HRIS.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController(IPositionRepository positionRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PositionResponse>>> GetAll(CancellationToken ct)
        => Ok(await positionRepository.GetResponseAsync(ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePositionRequest request, CancellationToken ct)
    {
        var position = request.ToEntity();
        await positionRepository.AddAsync(position, ct);
        await positionRepository.SaveChangesAsync(ct);

        return Created($"/api/positions/{position.Id}", new { id = position.Id });
    }
}
