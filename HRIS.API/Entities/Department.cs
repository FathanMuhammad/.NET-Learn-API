using System.Diagnostics.Contracts;

namespace HRIS.API.Entities;

public class Department
{
    public Guid Id { get; set; }
    public String Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    // Foreign Keys
    public Guid? ParentDepartmentId { get; set; }


    // Relationships seperti many to many dsbnya
    public Department? ParentDepartment { get; set; } // 
    public ICollection<Department> SubDepartments { get; set; } = [];   // Self Reference -  satu department bisa punya banyak sub-department
    public ICollection<Employee> Employees { get; set; } = [];
}
