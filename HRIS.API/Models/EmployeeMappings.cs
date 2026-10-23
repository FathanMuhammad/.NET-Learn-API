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
}