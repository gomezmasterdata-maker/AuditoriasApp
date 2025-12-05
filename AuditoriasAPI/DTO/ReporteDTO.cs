namespace AuditoriasAPI.DTO;

public class ReporteDTO
{
    public int AuditoriaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Responsable { get; set; } = string.Empty;
    public int HallazgosBajos { get; set; }
    public int HallazgosMedios { get; set; }
    public int HallazgosAltos { get; set; }
    public int TotalHallazgos { get; set; }
}
