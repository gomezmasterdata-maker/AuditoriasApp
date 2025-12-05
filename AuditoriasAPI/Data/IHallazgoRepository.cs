using AuditoriasAPI.Models;

namespace AuditoriasAPI.Data;

public interface IHallazgoRepository
{
    Task<IEnumerable<Hallazgo>> GetByAuditoriaIdAsync(int auditoriaId);
    Task<Hallazgo?> GetByIdAsync(int id);
    Task AddAsync(Hallazgo hallazgo);
    Task DeleteAsync(Hallazgo hallazgo);
    Task SaveChangesAsync();
    Task UpdateAsync(Hallazgo hallazgo);
}