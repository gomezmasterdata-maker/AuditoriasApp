using Microsoft.AspNetCore.Mvc;
using AuditoriasAPI.Data;
using AuditoriasAPI.DTO;


namespace AuditoriasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly IAuditoriaRepository _repository;

    public ReportesController(IAuditoriaRepository repository)
    {
        _repository = repository;
    }

    // GET: api/reportes/dashboard?inicio=2023-01-01&fin=2023-12-31
    [HttpGet("dashboard")]
    public async Task<ActionResult<IEnumerable<ReporteDTO>>> GetDashboard(
        [FromQuery] DateTime? inicio,
        [FromQuery] DateTime? fin)
    {
        var datos = await _repository.GetReporteFinalizadasAsync(inicio, fin);
        return Ok(datos);
    }
}