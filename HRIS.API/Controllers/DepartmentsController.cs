using HRIS.API.Data;
using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(HrisDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAll(CancellationToken ct)
    {
        var departments = await context.Departments
            .AsNoTracking()
            .Include(d => d.ParentDepartment)
            .ToListAsync(ct);

        return Ok(departments.Select(d => d.ToResponse()));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DepartmentResponse>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var department = await context.Departments
            .AsNoTracking()
            .Include(d => d.ParentDepartment)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        return department is null
            ? NotFound()
            : Ok(department.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponse>> Create(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken ct)
    {
        if (request.ParentDepartmentId.HasValue)
        {
            var parentExists = await context.Departments
                .AnyAsync(d => d.Id == request.ParentDepartmentId.Value, ct);

            if (!parentExists)
            {
                return BadRequest($"Parent department with Id '{request.ParentDepartmentId.Value}' was not found.");
            }
        }

        var department = new Department
        {
            Id = Guid.CreateVersion7(),
            Code = request.Code,
            Name = request.Name,
            ParentDepartmentId = request.ParentDepartmentId
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync(ct);

        if (department.ParentDepartmentId.HasValue)
        {
            await context.Entry(department)
                .Reference(d => d.ParentDepartment)
                .LoadAsync(ct);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            department.ToResponse());
    }
}
