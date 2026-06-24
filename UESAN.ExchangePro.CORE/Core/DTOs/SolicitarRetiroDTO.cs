namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class SolicitarRetiroDTO
    {
        public int IdMoneda { get; set; }
        public decimal Monto { get; set; }
        public string MetodoRetiro { get; set; } = null!;
        public string CuentaDestino { get; set; } = null!;
        public string Titular { get; set; } = null!;
    }
}