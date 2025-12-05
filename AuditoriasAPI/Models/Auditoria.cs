namespace AuditoriasAPI.Models;

public class Auditoria
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; } 
    public string AreaAuditada { get; set; } = string.Empty;
    public string Estado { get; set; } = "Pendiente";
    public int ResponsableId { get; set; }
    public Responsable? Responsable { get; set; }
    public ICollection<Hallazgo> Hallazgos { get; set; } = new List<Hallazgo>();
}
