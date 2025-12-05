using AuditoriasAPI.Data;
using AuditoriasAPI.DTO;
using AuditoriasAPI.Models;
using AuditoriasAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuditoriasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriasController : ControllerBase
{
    private readonly IAuditoriaService _service;
    private readonly IAuditoriaRepository _repository;

    // Inyectamos ambos para separar Lectura (Repo) de Escritura (Service)
    public AuditoriasController(IAuditoriaService service, IAuditoriaRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    // GET: api/auditorias?inicio=2023-01-01&fin=2023-12-31&estado=Pendiente
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Auditoria>>> GetAuditorias(
        [FromQuery] DateTime? inicio,
        [FromQuery] DateTime? fin,
        [FromQuery] string? estado,
        [FromQuery] int? responsableId)
    {
        var resultados = await _repository.GetByFilterAsync(inicio, fin, estado, responsableId);
        return Ok(resultados);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Auditoria>> GetAuditoria(int id)
    {
        var auditoria = await _repository.GetByIdAsync(id);
        if (auditoria == null) return NotFound();
        return Ok(auditoria);
    }

    // POST: api/auditorias
    [HttpPost]
    public async Task<ActionResult<Auditoria>> PostAuditoria(AuditoriaCreateDto dto)
    {
        var nuevaAuditoria = await _service.CreateAuditoriaAsync(dto);
        return CreatedAtAction(nameof(GetAuditorias), new { id = nuevaAuditoria.Id }, nuevaAuditoria);
    }

    // PUT: api/auditorias/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuditoria(int id, AuditoriaUpdateDto dto)
    {
        try
        {
            await _service.UpdateAuditoriaAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("La auditoría no existe.");
        }
        catch (InvalidOperationException ex)
        {
            // Retornamos 400 Bad Request cuando falla la validación de negocio (Estado != Pendiente)
            return BadRequest(ex.Message);
        }
    }

    // PATCH: api/auditorias/5/estado
    // Endpoint específico para cambiar estado
    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> PatchEstado(int id, [FromBody] string nuevoEstado)
    {
        var auditoria = await _repository.GetByIdAsync(id);
        if (auditoria == null) return NotFound();

        auditoria.Estado = nuevoEstado;
        await _repository.UpdateAsync(auditoria);
        await _repository.SaveChangesAsync();

        return NoContent();
    }
}