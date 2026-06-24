namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class RetiroResponseDTO
    {
        public long IdRetiro { get; set; }
        public string? MonedaCodigo { get; set; }
        public decimal Monto { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaRetiro { get; set; }
    }
}
