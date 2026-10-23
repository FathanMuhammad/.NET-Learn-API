using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Data.Repositories;

public class EmployeeRepository(HrisDbContext context) : Repository<Employee>(context), IEmployeeRepository
{
    private static IQueryable<EmployeeResponse> ProjectToResponse(IQueryable<Employee> query)
        => query.Select(e => e.ToResponse(
            e.Department.Name,
            e.Position.Title,
            managerFullName: e.Manager != null
                ? e.Manager.FullName
                : null));

    public async Task<IReadOnlyCollection<EmployeeResponse>> GetResponseAsync(CancellationToken ct = default)
        => await ProjectToResponse(
            Context.Employees
                .AsNoTracking()
                .OrderBy(e => e.FullName)
        ).ToListAsync(ct);

    public async Task<EmployeeResponse?> GetResponseAsync(Guid id, CancellationToken ct = default)
        => await ProjectToResponse(
            Context.Employees
                .AsNoTracking()
                .Where(e => e.Id == id)
        ).FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyCollection<Employee>> GetDirectReportsAsync(Guid managerId, CancellationToken ct = default)
        => await Context.Employees.Where(e => e.ManagerId == managerId).ToListAsync(ct);
}