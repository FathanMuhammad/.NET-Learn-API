using HRIS.API.Entities;

namespace HRIS.API.Models;

public static class DepartmentMappings
{
    public static DepartmentResponse ToResponse(this Department department) =>
        new(
            department.Id,
            department.Code,
            department.Name,
            department.ParentDepartmentId,
            department.ParentDepartment?.Name
        );
}
