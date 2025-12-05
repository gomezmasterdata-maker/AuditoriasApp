using AuditoriasAPI.DTO;
using AuditoriasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditoriasAPI.Data;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly AppDbContext _context;

    public AuditoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Auditoria>> GetAllAsync()
    {
        // Incluye al Responsable
        return await _context.Auditorias.Include(a => a.Responsable).ToListAsync();
    }

    public async Task<Auditoria?> GetByIdAsync(int id)
    {
        return await _context.Auditorias
                             .Include(a => a.Responsable)
                             .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Auditoria auditoria)
    {
        await _context.Auditorias.AddAsync(auditoria);
    }

    public Task UpdateAsync(Auditoria auditoria)
    {
        _context.Entry(auditoria).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Auditoria>> GetByFilterAsync(DateTime? inicio, DateTime? fin, string? estado, int? responsableId)
    {
        var query = _context.Auditorias.Include(a => a.Responsable).AsQueryable();

        if (inicio.HasValue)
            query = query.Where(a => a.FechaInicio >= inicio.Value);

        if (fin.HasValue)
            query = query.Where(a => a.FechaInicio <= fin.Value);

        if (!string.IsNullOrEmpty(estado))
            query = query.Where(a => a.Estado == estado);

        if (responsableId.HasValue)
            query = query.Where(a => a.ResponsableId == responsableId.Value);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<ResumenAuditoriaDTO>> GetResumenReporteAsync()
    {
        // Aquí consumimos la Vista SQL directamente.
        // Mapeo directo a DTO desde la vista
        var resultado = await _context.Database
            .SqlQuery<ResumenAuditoriaDTO>($"SELECT * FROM dbo.vw_ResumenAuditoriasFinalizadas")
            .ToListAsync();

        return resultado;
    }

    // Implementación
    public async Task<IEnumerable<ReporteDTO>> GetReporteFinalizadasAsync(DateTime? inicio, DateTime? fin)
    {
        // Consulta base sobre la vista
        var query = _context.Database.SqlQuery<ReporteDTO>($"SELECT * FROM dbo.vw_ResumenAuditoriasFinalizadas");
        var resultados = await query.ToListAsync();

        if (inicio.HasValue)
            resultados = resultados.Where(x => x.FechaInicio >= inicio.Value).ToList();

        if (fin.HasValue)
            resultados = resultados.Where(x => x.FechaInicio <= fin.Value).ToList();

        return resultados;
    }
}