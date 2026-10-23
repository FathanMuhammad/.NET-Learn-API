using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Data.Repositories;

public class DepartmentRepository(HrisDbContext context) : Repository<Department>(context), IDepartmentRepository
{
    private IQueryable<DepartmentResponse> Responses()
        => Context.Departments
            .AsNoTracking()
            .Include(d => d.ParentDepartment)
            .Select(d => d.ToResponse());

    public async Task<IReadOnlyCollection<DepartmentResponse>> GetResponseAsync(CancellationToken ct = default)
        => await Responses().ToListAsync(ct);

    public async Task<DepartmentResponse?> GetResponseByIdAsync(Guid id, CancellationToken ct = default)
        => await Responses().FirstOrDefaultAsync(d => d.Id == id, ct);
}
