using AuditoriasAPI.Models;
using AuditoriasAPI.Data;
using AuditoriasAPI.DTO;

namespace AuditoriasAPI.Services;

public class HallazgoService
{
    private readonly IHallazgoRepository _hallazgoRepo;
    private readonly IAuditoriaRepository _auditoriaRepo; 

    public HallazgoService(IHallazgoRepository hallazgoRepo, IAuditoriaRepository auditoriaRepo)
    {
        _hallazgoRepo = hallazgoRepo;
        _auditoriaRepo = auditoriaRepo;
    }

    public async Task<Hallazgo> CreateHallazgoAsync(Hallazgo hallazgo)
    {
        var auditoria = await _auditoriaRepo.GetByIdAsync(hallazgo.AuditoriaId);
        if (auditoria == null) throw new KeyNotFoundException("Auditoría no encontrada");

        if (auditoria.Estado == "Finalizada")
            throw new InvalidOperationException("No se pueden agregar hallazgos a una auditoría finalizada.");

        await _hallazgoRepo.AddAsync(hallazgo);
        await _hallazgoRepo.SaveChangesAsync();
        return hallazgo;
    }

    public async Task DeleteHallazgoAsync(int id)
    {
        var hallazgo = await _hallazgoRepo.GetByIdAsync(id);
        if (hallazgo == null) throw new KeyNotFoundException("Hallazgo no encontrado");

        var auditoria = await _auditoriaRepo.GetByIdAsync(hallazgo.AuditoriaId);

        if (auditoria.Estado != "En Proceso")
        {
            throw new InvalidOperationException("Solo se pueden eliminar hallazgos cuando la auditoría está 'En Proceso'.");
        }

        await _hallazgoRepo.DeleteAsync(hallazgo);
        await _hallazgoRepo.SaveChangesAsync();
    }

    public async Task UpdateHallazgoAsync(int id, Hallazgo hallazgoActualizado)
    {
        var hallazgoExistente = await _hallazgoRepo.GetByIdAsync(id);
        if (hallazgoExistente == null) throw new KeyNotFoundException("Hallazgo no encontrado");

        var auditoria = await _auditoriaRepo.GetByIdAsync(hallazgoExistente.AuditoriaId);

        if (auditoria.Estado != "En Proceso")
        {
            throw new InvalidOperationException("Solo se pueden editar hallazgos cuando la auditoría está 'En Proceso'.");
        }

        hallazgoExistente.Descripcion = hallazgoActualizado.Descripcion;
        hallazgoExistente.Tipo = hallazgoActualizado.Tipo;
        hallazgoExistente.Severidad = hallazgoActualizado.Severidad;
        hallazgoExistente.FechaDeteccion = hallazgoActualizado.FechaDeteccion;

        await _hallazgoRepo.UpdateAsync(hallazgoExistente);
        await _hallazgoRepo.SaveChangesAsync();
    }
}