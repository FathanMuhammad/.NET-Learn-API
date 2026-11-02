using HRIS.API.Common.Exceptions;
using HRIS.API.Common.Security;
using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Features.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Features.Employees.Queries.GetDirectReports;

public record GetDirectReportsQuery(Guid ManagerId) : IRequest<IReadOnlyCollection<EmployeeListDto>>;

public class GetDirectReportsQueryHandler(HrisDbContext db, ICurrentUser currentUser)
    : IRequestHandler<GetDirectReportsQuery, IReadOnlyCollection<EmployeeListDto>>
{
    public async Task<IReadOnlyCollection<EmployeeListDto>> Handle(GetDirectReportsQuery query, CancellationToken ct)
    {
        if (!currentUser.IsHrOrAdmin && currentUser.EmployeeId != query.ManagerId)
            throw new ForbiddenAccessException("You can only view your own direct reports.");

        return await db.Employees
            .AsNoTracking()
            .Where(e => e.ManagerId == query.ManagerId && e.Status == EmployeeStatus.Active)
            .OrderBy(e => e.FullName)
            .Select(e => new EmployeeListDto(
                e.Id, e.EmployeeNumber, e.FullName, e.Department.Name, e.Position.Title,
                e.Manager != null ? e.Manager.FullName : null, e.Status))
            .ToListAsync(ct);
    }
}
