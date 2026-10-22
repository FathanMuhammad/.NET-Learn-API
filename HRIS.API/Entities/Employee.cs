namespace HRIS.API.Entities;

public enum EmployeeStatus
{
    Active, 
    Terminated
}

public class Employee
{
    // Field Table
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly HireDate { get; set; }
    public EmployeeStatus EmployeeStatus { get; set; } = EmployeeStatus.Active;
    //public DateOnly CreatedAt { get; set; }
    public DateOnly? TerminatedAt { get; set; }
    
    /// Foreign Keys
    public Guid DepartmentId { get; set; } 
    public Guid PositionId { get; set; } 
    public Guid? ManagerId { get; set; } 

    // Relationships 
    public Department Department { get; set; } = null!;
    public Position Position { get; set; } = null!;
    public Employee Manager { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = [];

    /*   tambahkan CreatedAt ke entah service/controller
     *   otomatis terbuat saat create/Post
     *   harus bungkus perintahnya ditempat lain
     *   
     *   jadi setiap create, bakal muncul CreatedAt, dan muncul saat perintah API GET
    */
}
