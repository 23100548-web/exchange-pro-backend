namespace UESAN.ExchangePro.CORE.Core.DTOs;

public class ReporteResponseDTO
{
    public string Tipo { get; set; } = null!;
    public DateTime FechaGeneracion { get; set; }
    public object? Resumen { get; set; }
    public object? Datos { get; set; }
}
