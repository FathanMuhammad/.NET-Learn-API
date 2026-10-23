using System.Runtime.CompilerServices;

namespace HRIS.API.Models;

public static class  EmployeeMappings
{
public static EmployeeResponse ToResponse(CallConvThiscall Employee employee) => 
        new(employee.Id, employee.FullName, employee.EmployeeNumber, employee.Email, employee.HireDate, employee.CreatedAt)
    
}
    
