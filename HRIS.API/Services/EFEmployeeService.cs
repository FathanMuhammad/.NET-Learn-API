using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Services;

public class EFEmployeeService(HrisDbContext context, TimeProvider timeProvider) : IEmployeeService
{
    public async Task<IReadOnlyCollection<EmployeeResponse>> GetAllAsync(CancellationToken ct = default)
        => await context.Employees
            .AsNoTracking() // read-only: karena tidak ada perubahan tracking yang dibutuhkan
            .OrderBy(e => e.FullName)
            .Select(e => e.ToResponse())
            .ToListAsync(ct);

    public async Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Employees
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => e.ToResponse())
            .FirstOrDefaultAsync(ct);

    public async Task<Guid> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
    {
        var employee = new Employee
        {
            Id = Guid.CreateVersion7(),
            EmployeeNumber = request.EmployeeNumber,
            FullName = request.FullName,
            Email = request.Email.ToLowerInvariant(),
            HireDate = request.HireDate,
            CreatedAt = timeProvider.GetUtcNow(),
            DepartmentId = request.DepartmentId,
            PositionId = request.PositionId,
            ManagerId = request.ManagerId
        };

        context.Employees.Add(employee); // state
        await context.SaveChangesAsync(ct);

        return employee.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default)
    {
        var employee = await context.Employees.FindAsync([id], ct);
        if (employee is null)
            return false;

        employee.FullName = request.FullName;
        employee.DepartmentId = request.DepartmentId;
        employee.PositionId = request.PositionId;
        employee.ManagerId = request.ManagerId;

        await context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var employee = await context.Employees.FindAsync([id], ct);
        if (employee is null)
            return false;

        context.Employees.Remove(employee);
        await context.SaveChangesAsync(ct);
        return true;
    }
}
