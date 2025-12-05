namespace AuditoriasAPI.DTO;

public record ResumenAuditoriaDTO(
    int AuditoriaId,
    string Titulo,
    DateTime FechaInicio,
    string Responsable,
    int HallazgosBajos,
    int HallazgosMedios,
    int HallazgosAltos,
    int TotalHallazgos
);