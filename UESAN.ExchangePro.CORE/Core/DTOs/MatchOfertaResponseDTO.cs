namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class MatchOfertaResponseDTO
    {
        public long IdOferta { get; set; }
        public string? NombreUsuario { get; set; }
        public decimal Reputacion { get; set; }
        public int TotalOperaciones { get; set; }
        public string? MonedaEntregaCode { get; set; }
        public string? MonedaRecibeCode { get; set; }
        public decimal TasaCambio { get; set; }
        public decimal MontoOfertado { get; set; }
        public decimal MontoMinimo { get; set; }
        public string? TipoOperacion { get; set; }
        public List<string> MetodosPago { get; set; } = new();
        public decimal DiferenciaTasa { get; set; }
    }
}