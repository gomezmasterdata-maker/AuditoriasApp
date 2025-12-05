using AuditoriasAPI.Data;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Services;

public class ResponsableService
{
    private readonly IResponsableRepository _repository;

    public ResponsableService(IResponsableRepository repository)
    {
        _repository = repository;
    }

    public async Task UpdateResponsableAsync(int id, Responsable responsableActualizado)
    {
        
        var responsableDb = await _repository.GetByIdAsync(id);
        if (responsableDb == null) throw new KeyNotFoundException("Responsable no encontrado");

        bool tieneAuditorias = await _repository.TieneAuditoriasAsignadas(id);
        if (tieneAuditorias)
        {
            throw new InvalidOperationException("No se puede editar el responsable porque ya tiene auditorías asignadas (Integridad Histórica).");
        }

        responsableDb.Nombre = responsableActualizado.Nombre;
        responsableDb.Correo = responsableActualizado.Correo;
        responsableDb.Area = responsableActualizado.Area;

        await _repository.UpdateAsync(responsableDb);
        await _repository.SaveChangesAsync();
    }

    public async Task CreateResponsableAsync(Responsable responsable)
    {
        await _repository.AddAsync(responsable);
        await _repository.SaveChangesAsync();
    }
}