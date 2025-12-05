using AuditoriasAPI.DTO;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Services;

public interface IAuditoriaService
{
    Task<Auditoria> CreateAuditoriaAsync(AuditoriaCreateDto dto);
    Task UpdateAuditoriaAsync(int id, AuditoriaUpdateDto dto);
}