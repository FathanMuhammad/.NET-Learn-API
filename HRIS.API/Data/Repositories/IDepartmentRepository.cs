using HRIS.API.Entities;
using HRIS.API.Models;
using YourProject.Domain.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IReadOnlyCollection<DepartmentResponse>> GetResponseAsync(CancellationToken ct = default);
}