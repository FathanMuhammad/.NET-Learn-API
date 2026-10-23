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

    public async Task<TerminateResult> TerminateAsync(Guid id, CancellationToken ct = default)
    {
        // 1. check employee if exist (using id)
        var employee = await uow.Employees.GetByIdAsync(id, ct);
        if (employee is null)
            return TerminateResult.NotFound;

        if (employee.EmployeeStatus == EmployeeStatus.Terminated)
            return TerminateResult.AlreadyTerminated;

        // 2. change status(Terminated) & catat waktu terminate nya (TerminatedAt)
        employee.EmployeeStatus = EmployeeStatus.Terminated;
        employee.TerminatedAt = DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);

        // 3. memperbaiki every direct report moves up to terminated employee manager | may be null, direct report itu naik keatas, contoh atasan resign, otomatis atasan ganti ke yg atas
        var directReports = await uow.Employees.GetDirectReportsAsync(employee.Id, ct);
        foreach (var report in directReports)
        {
            report.ManagerId = employee.ManagerId;
        }

        // 4. save changes employee
        await uow.SaveChangesAsync(ct);

        return TerminateResult.Terminated;
    }
}
