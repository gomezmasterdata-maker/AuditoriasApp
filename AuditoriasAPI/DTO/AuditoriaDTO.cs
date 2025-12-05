namespace AuditoriasAPI.DTO;

public record AuditoriaCreateDto(string Titulo, DateTime FechaInicio, string AreaAuditada, int ResponsableId);
public record AuditoriaUpdateDto(string Titulo, string AreaAuditada, DateTime? FechaFin);