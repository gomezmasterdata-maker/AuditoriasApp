namespace AuditoriasAPI.Models;

public class Hallazgo
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Severidad { get; set; } = string.Empty;
    public DateTime FechaDeteccion { get; set; }
    public int AuditoriaId { get; set; }
    public Auditoria? Auditoria { get; set; }
}
