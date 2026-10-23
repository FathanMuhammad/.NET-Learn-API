using HRIS.API.Entities;

namespace HRIS.API.Models;

public static class EmployeeMappings
{
    public static EmployeeResponse ToResponse(
        this Employee employee,
        string departmentName,
        string positionTitle,
        string? managerFullName = null) =>
        new(employee.Id,
            employee.EmployeeNumber,
            employee.FullName,
            employee.Email,
            employee.HireDate,
            employee.EmployeeStatus,
            departmentName,
            positionTitle,
            managerFullName,
            employee.CreatedAt);

    public static EmployeeResponse ToResponse(this Employee employee) =>
        employee.ToResponse(
            employee.Department?.Name ?? string.Empty,
            employee.Position?.Title ?? string.Empty,
            employee.Manager?.FullName);

    public static Employee ToEntity(
        this CreateEmployeeRequest request,
        DateTimeOffset createdAt,
        EmployeeStatus employeeStatus) =>
            new(Guid.CreateVersion7(),
            request.EmployeeNumber,
            request.FullName,
            request.Email,
            request.HireDate,
            createdAt,
            employeeStatus,
            null,
            request.DepartmentId,
            request.PositionId,
            request.ManagerId);

    public static Employee ToEntity(
        this Employee employee,
        UpdateEmployeeRequest request)
    {
        employee.EmployeeNumber = request.EmployeeNumber;
        employee.FullName = request.FullName;
        employee.Email = request.Email.ToLowerInvariant();
        employee.HireDate = request.HireDate;
        employee.DepartmentId = request.DepartmentId;
        employee.PositionId = request.PositionId;
        employee.ManagerId = request.ManagerId;
        return employee;
    }

}