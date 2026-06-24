namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class WalletResponseDTO
    {
        public long IdWallet { get; set; }
        public decimal SaldoDisponible { get; set; }
        public decimal SaldoRetenido { get; set; }
        public string Moneda { get; set; } = null!;
    }
}