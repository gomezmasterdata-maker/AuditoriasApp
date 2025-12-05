using Microsoft.AspNetCore.Mvc;
using AuditoriasAPI.Services;
using AuditoriasAPI.Data;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponsablesController : ControllerBase
{
    private readonly IResponsableRepository _repository;
    private readonly ResponsableService _service;

    public ResponsablesController(IResponsableRepository repository, ResponsableService service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Responsable>>> GetAll()
    {
        return Ok(await _repository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Responsable>> GetById(int id)
    {
        var resp = await _repository.GetByIdAsync(id);
        if (resp == null) return NotFound();
        return Ok(resp);
    }

    [HttpGet("{id}/tiene-auditorias")]
    public async Task<ActionResult<bool>> CheckDependencias(int id)
    {
        return Ok(await _repository.TieneAuditoriasAsignadas(id));
    }

    [HttpPost]
    public async Task<ActionResult<Responsable>> Create(Responsable responsable)
    {
        await _service.CreateResponsableAsync(responsable);
        return CreatedAtAction(nameof(GetById), new { id = responsable.Id }, responsable);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Responsable responsable)
    {
        if (id != responsable.Id) return BadRequest();

        try
        {
            await _service.UpdateResponsableAsync(id, responsable);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}