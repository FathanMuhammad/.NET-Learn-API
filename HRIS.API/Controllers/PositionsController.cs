using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController(HrisDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PositionResponse>>> GetAll(CancellationToken ct)
    {
        var positions = await context.Positions
            .AsNoTracking()
            .ToListAsync(ct);

        return Ok(positions.Select(p => p.ToResponse()));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PositionResponse>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var position = await context.Positions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        return position is null
            ? NotFound()
            : Ok(position.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<PositionResponse>> Create(
        [FromBody] CreatePositionRequest request,
        CancellationToken ct)
    {
        var position = new Position
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            Level = request.Level
        };

        context.Positions.Add(position);
        await context.SaveChangesAsync(ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = position.Id },
            position.ToResponse());
    }
}
