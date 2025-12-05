using AuditoriasAPI.DTO;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Data;

public interface IAuditoriaRepository
{
    Task<IEnumerable<Auditoria>> GetAllAsync();
    Task<Auditoria?> GetByIdAsync(int id);
    Task AddAsync(Auditoria auditoria);
    Task UpdateAsync(Auditoria auditoria);
    Task SaveChangesAsync();
    Task<IEnumerable<ResumenAuditoriaDTO>> GetResumenReporteAsync();
    Task<IEnumerable<Auditoria>> GetByFilterAsync(DateTime? inicio, DateTime? fin, string? estado, int? responsableId);
    Task<IEnumerable<ReporteDTO>> GetReporteFinalizadasAsync(DateTime? inicio, DateTime? fin);
}