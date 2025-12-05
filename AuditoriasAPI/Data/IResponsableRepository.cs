using AuditoriasAPI.Models;

namespace AuditoriasAPI.Data;

public interface IResponsableRepository
{
    Task<IEnumerable<Responsable>> GetAllAsync();
    Task<Responsable?> GetByIdAsync(int id);
    Task AddAsync(Responsable responsable);
    Task UpdateAsync(Responsable responsable);
    Task<bool> TieneAuditoriasAsignadas(int id);
    Task SaveChangesAsync();
}