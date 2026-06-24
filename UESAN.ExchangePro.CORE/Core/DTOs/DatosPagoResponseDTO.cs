namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class DatosPagoResponseDTO
    {
        public long IdDatoPago { get; set; }
        public string? Yape { get; set; }
        public string? Plin { get; set; }
        public int? IdBanco { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? Cci { get; set; }
        public string? BancoNombre { get; set; }
    }
}