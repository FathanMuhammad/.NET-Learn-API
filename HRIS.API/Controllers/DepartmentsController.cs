using HRIS.API.Data.Repositories;
using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController(IDepartmentRepository departmentRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAll(CancellationToken ct)
        => Ok(await departmentRepository.GetResponseAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DepartmentResponse>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var department = await departmentRepository.GetResponseByIdAsync(id, ct);
        return department is null
            ? NotFound()
            : Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponse>> Create(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken ct)
    {
        if (request.ParentDepartmentId.HasValue)
        {
            var parent = await departmentRepository.GetByIdAsync(request.ParentDepartmentId.Value, ct);
            if (parent is null)
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

        await departmentRepository.AddAsync(department, ct);
        await departmentRepository.SaveChangesAsync(ct);

        var response = await departmentRepository.GetResponseByIdAsync(department.Id, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            response);
    }
}
