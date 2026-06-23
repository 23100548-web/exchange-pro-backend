using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public class MonedaTipoCambioDTO
    {
        public string Codigo { get; set; } = "";
        public decimal Compra { get; set; }
        public decimal Venta { get; set; }
        public decimal Mid { get; set; }
        public string Direccion { get; set; } = "estable";
    }

    public class TipoCambioResponseDTO
    {
        public List<MonedaTipoCambioDTO> Monedas { get; set; } = new();
        public string Fecha { get; set; } = "";
    }

    public interface ITipoCambioService
    {
        Task<TipoCambioResponseDTO> GetTipoCambio();
    }
}