using HRIS.API.Data;
using HRIS.API.Data.Repositories;
using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Services;

public class EFEmployeeService(IUnitOfWork uow, TimeProvider timeProvider) : IEmployeeService
{
    public async Task<IReadOnlyCollection<EmployeeResponse>> GetAllAsync(CancellationToken ct = default)
        => await uow.Employees.GetResponseAsync(ct);

    public Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => uow.Employees.GetResponseAsync(id, ct);

    public async Task<Guid> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
    {
        var employee = request.ToEntity(
            timeProvider.GetUtcNow(),
            EmployeeStatus.Active
            );

        await uow.Employees.AddAsync(employee, ct);
        await uow.SaveChangesAsync(ct);
        return employee.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default)
    {
        var employee = await uow.Employees.GetByIdAsync(id, ct);
        if (employee is null)
            return false;

        employee.ToEntity(request);
        await uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var employee = await uow.Employees.GetByIdAsync(id, ct);
        if (employee is null)
            return false;

        uow.Employees.Delete(employee);
        await uow.SaveChangesAsync(ct);
        return true;
    }

    public Task<TerminateResult> TerminateAsync(Guid id, CancellationToken ct = default)
    {

    }
}

//public class EFEmployeeService(HrisDbContext context, TimeProvider timeProvider) : IEmployeeService
//{
//    public async Task<IReadOnlyCollection<EmployeeResponse>> GetAllAsync(CancellationToken ct = default)
//        => await context.Employees // DbSet<Employee>
//            .AsNoTracking() // IQueryable<Employee>
//            .OrderBy(e => e.FullName) // IOrderedQueryable<Employee>
//            .Select(e => e.ToResponse(
//                e.Department.Name,
//                e.Position.Title,
//                managerFullName: e.Manager != null
//                    ? e.Manager.FullName
//                    : null)) // IQueryable<EmployeeResponse>
//            .ToListAsync(ct); // Task<List<...>>

//    public async Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
//        => await context.Employees // DbSet<Employee>
//            .AsNoTracking()
//            .Where(e => e.Id == id) // IQueryable<Employee>
//            .Select(e => e.ToResponse(
//                e.Department.Name,
//                e.Position.Title,
//                managerFullName: e.Manager != null
//                    ? e.Manager.FullName
//                    : null)) // IQueryable<EmployeeResponse>
//            .FirstOrDefaultAsync(ct);

//    public async Task<Guid> CreateAsync(CreateEmployeeRequest request, CancellationToken ct = default)
//    {
//        var employee = new Employee
//        {
//            Id = Guid.CreateVersion7(),
//            EmployeeNumber = request.EmployeeNumber,
//            FullName = request.FullName,
//            Email = request.Email.ToLowerInvariant(),
//            HireDate = request.HireDate,
//            CreatedAt = timeProvider.GetUtcNow(),
//            DepartmentId = request.DepartmentId,
//            PositionId = request.PositionId,
//            ManagerId = request.ManagerId
//        };

//        context.Employees.Add(employee); // state
//        await context.SaveChangesAsync(ct);

//        return employee.Id;
//    }

//    public async Task<bool> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct = default)
//    {
//        var employee = await context.Employees.FindAsync([id], ct);
//        if (employee is null)
//            return false;

//        employee.FullName = request.FullName;
//        employee.DepartmentId = request.DepartmentId;
//        employee.PositionId = request.PositionId;
//        employee.ManagerId = request.ManagerId;

//        await context.SaveChangesAsync(ct);
//        return true;
//    }

//    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
//    {
//        var employee = await context.Employees.FindAsync([id], ct);
//        if (employee is null)
//            return false;

//        context.Employees.Remove(employee);
//        await context.SaveChangesAsync(ct);
//        return true;
//    }

//    Task<IReadOnlyCollection<EmployeeResponse>> IEmployeeService.GetAllAsync(CancellationToken ct)
//    {
//        throw new NotImplementedException();
//    }

//    Task<EmployeeResponse?> IEmployeeService.GetByIdAsync(Guid id, CancellationToken ct)
//    {
//        throw new NotImplementedException();
//    }

//    Task<Guid> IEmployeeService.CreateAsync(CreateEmployeeRequest request, CancellationToken ct)
//    {
//        throw new NotImplementedException();
//    }

//    Task<bool> IEmployeeService.UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken ct)
//    {
//        throw new NotImplementedException();
//    }

//    Task<bool> IEmployeeService.DeleteAsync(Guid id, CancellationToken ct)
//    {
//        throw new NotImplementedException();
//    }

//    Task<TerminateResult> IEmployeeService.TerminateAsync(Guid id, CancellationToken ct)
//    {
//        throw new NotImplementedException();
//    }
//}
