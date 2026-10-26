using HRIS.API.Features.Reports.Queries.GetHeadcountByDepartment;
using HRIS.API.Features.Reports.Queries.GetHeadcountByPosition;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(ISender sender) : ControllerBase
{
    [HttpGet("headcount-by-department")]
    public async Task<ActionResult<IReadOnlyCollection<HeadcountByDepartmentDto>>> HeadcountByDepartment(CancellationToken ct)
        => Ok(await sender.Send(new GetHeadcountByDepartmentQuery(), ct));

    [HttpGet("headcount-by-position")]
    public async Task<ActionResult<IReadOnlyCollection<HeadcountByPositionDto>>> HeadcountByPosition(CancellationToken ct)
        => Ok(await sender.Send(new GetHeadcountByPositionQuery(), ct));
}
