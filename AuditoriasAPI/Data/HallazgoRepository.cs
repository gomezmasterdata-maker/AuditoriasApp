using Microsoft.EntityFrameworkCore;
using AuditoriasAPI.Models;

namespace AuditoriasAPI.Data;

public class HallazgoRepository : IHallazgoRepository
{
    private readonly AppDbContext _context;

    public HallazgoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Hallazgo>> GetByAuditoriaIdAsync(int auditoriaId)
    {
        return await _context.Hallazgos
                             .Where(h => h.AuditoriaId == auditoriaId)
                             .ToListAsync();
    }

    public async Task<Hallazgo?> GetByIdAsync(int id)
    {
        return await _context.Hallazgos.FindAsync(id);
    }

    public async Task AddAsync(Hallazgo hallazgo)
    {
        await _context.Hallazgos.AddAsync(hallazgo);
    }

    public Task DeleteAsync(Hallazgo hallazgo)
    {
        _context.Hallazgos.Remove(hallazgo);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Hallazgo hallazgo)
    {
        _context.Entry(hallazgo).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}