namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class MovimientoWalletResponseDTO
    {
        public long IdMovimiento { get; set; }
        public string? MonedaCodigo { get; set; }
        public string? TipoOperacion { get; set; }
        public decimal? Monto { get; set; }
        public string? Resultado { get; set; }
        public string? ReferenciaTipo { get; set; }
        public long? ReferenciaId { get; set; }
        public DateTime? FechaMovimiento { get; set; }
    }
}