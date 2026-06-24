namespace UESAN.ExchangePro.CORE.Core.DTOs;

public class ReporteRequestDTO
{
    public string Tipo { get; set; } = null!;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? IdMoneda { get; set; }
    public string? Estado { get; set; }
}
