using Microsoft.EntityFrameworkCore;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Data;

public class ResponsableRepository : IResponsableRepository
{
    private readonly AppDbContext _context;

    public ResponsableRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Responsable>> GetAllAsync()
    {
        return await _context.Responsables.ToListAsync();
    }

    public async Task<Responsable?> GetByIdAsync(int id)
    {
        return await _context.Responsables.FindAsync(id);
    }

    public async Task AddAsync(Responsable responsable)
    {
        await _context.Responsables.AddAsync(responsable);
    }

    public Task UpdateAsync(Responsable responsable)
    {
        _context.Entry(responsable).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task<bool> TieneAuditoriasAsignadas(int id)
    {
        // Verifica si existe al menos una auditoría con este responsable
        return await _context.Auditorias.AnyAsync(a => a.ResponsableId == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}