namespace HRIS.API.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

 /*   tambahkan CreatedAt ke entah service/controller
  *   otomatis terbuat saat create/Post
  *   harus bungkus perintahnya ditempat lain
  *   
  *   jadi setiap create, bakal muncul CreatedAt, dan muncul saat perintah API GET
 */       
}
