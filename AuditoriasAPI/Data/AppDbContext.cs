using AuditoriasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuditoriasAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Auditoria> Auditorias { get; set; }
    public DbSet<Hallazgo> Hallazgos { get; set; }
    public DbSet<Responsable> Responsables { get; set; }
}