using AuditoriasAPI.Data;
using AuditoriasAPI.DTO;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _repository;

    public AuditoriaService(IAuditoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Auditoria> CreateAuditoriaAsync(AuditoriaCreateDto dto)
    {
        var auditoria = new Auditoria
        {
            Titulo = dto.Titulo,
            FechaInicio = dto.FechaInicio,
            AreaAuditada = dto.AreaAuditada,
            ResponsableId = dto.ResponsableId,
            Estado = "Pendiente" // Regla por defecto
        };

        await _repository.AddAsync(auditoria);
        await _repository.SaveChangesAsync();
        return auditoria;
    }

    public async Task UpdateAuditoriaAsync(int id, AuditoriaUpdateDto dto)
    {
        var auditoria = await _repository.GetByIdAsync(id);
        if (auditoria == null) throw new KeyNotFoundException("Auditoría no encontrada");

        if (auditoria.Estado != "Pendiente")
        {
            throw new InvalidOperationException("Error: Solo se pueden editar auditorías en estado 'Pendiente'.");
        }

        auditoria.Titulo = dto.Titulo;
        auditoria.AreaAuditada = dto.AreaAuditada;
        if (dto.FechaFin.HasValue) auditoria.FechaFin = dto.FechaFin.Value;

        await _repository.UpdateAsync(auditoria);
        await _repository.SaveChangesAsync();
    }
}