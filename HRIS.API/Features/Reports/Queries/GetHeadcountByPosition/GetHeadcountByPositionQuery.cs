using HRIS.API.Data;
using HRIS.API.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Reports.Queries.GetHeadcountByPosition;

public record HeadcountByPositionDto(string PositionTitle, int Headcount);

public record GetHeadcountByPositionQuery : IRequest<IReadOnlyCollection<HeadcountByPositionDto>>;

public class GetHeadcountByPositionQueryHandler(HrisDbContext db)
    : IRequestHandler<GetHeadcountByPositionQuery, IReadOnlyCollection<HeadcountByPositionDto>>
{
    public async Task<IReadOnlyCollection<HeadcountByPositionDto>> Handle(
        GetHeadcountByPositionQuery query, CancellationToken ct)
        => await db.Employees
            .AsNoTracking()
            .Where(e => e.Status == EmployeeStatus.Active)
            .GroupBy(e => e.Position.Title)
            .OrderByDescending(g => g.Count()).ThenBy(g => g.Key)
            .Select(g => new HeadcountByPositionDto(g.Key, g.Count()))
            .ToListAsync(ct);
}
