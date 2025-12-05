using Microsoft.AspNetCore.Mvc;
using AuditoriasAPI.Services;
using AuditoriasAPI.Data;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HallazgosController : ControllerBase
{
    private readonly HallazgoService _service;
    private readonly IHallazgoRepository _repository;

    public HallazgosController(HallazgoService service, IHallazgoRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    // GET: api/hallazgos/por-auditoria/5
    [HttpGet("por-auditoria/{auditoriaId}")]
    public async Task<ActionResult<IEnumerable<Hallazgo>>> GetByAuditoria(int auditoriaId)
    {
        return Ok(await _repository.GetByAuditoriaIdAsync(auditoriaId));
    }

    // POST: api/hallazgos
    [HttpPost]
    public async Task<ActionResult<Hallazgo>> PostHallazgo(Hallazgo hallazgo)
    {
        try
        {
            var creado = await _service.CreateHallazgoAsync(hallazgo);
            return Ok(creado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/hallazgos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHallazgo(int id)
    {
        try
        {
            await _service.DeleteHallazgoAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Hallazgo>> GetHallazgo(int id)
    {
        var h = await _repository.GetByIdAsync(id);
        if (h == null) return NotFound();
        return Ok(h);
    }

    // PUT: api/hallazgos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHallazgo(int id, Hallazgo hallazgo)
    {
        if (id != hallazgo.Id) return BadRequest("ID no coincide");

        try
        {
            await _service.UpdateHallazgoAsync(id, hallazgo);
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