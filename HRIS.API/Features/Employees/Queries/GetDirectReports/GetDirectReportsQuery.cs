using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Features.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Employees.Queries.GetDirectReports;

public record GetDirectReportsQuery(Guid ManagerId) : IRequest<IReadOnlyCollection<EmployeeListDto>>;

public class GetDirectReportsQueryHandler(HrisDbContext db)
    : IRequestHandler<GetDirectReportsQuery, IReadOnlyCollection<EmployeeListDto>>
{
    public async Task<IReadOnlyCollection<EmployeeListDto>> Handle(GetDirectReportsQuery query, CancellationToken ct)
        => await db.Employees
            .AsNoTracking()
            .Where(e => e.ManagerId == query.ManagerId && e.Status == EmployeeStatus.Active)
            .OrderBy(e => e.FullName)
            .Select(e => new EmployeeListDto(
                e.Id, e.EmployeeNumber, e.FullName, e.Department.Name, e.Position.Title,
                e.Manager != null ? e.Manager.FullName : null, e.Status))
            .ToListAsync(ct);
}
