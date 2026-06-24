using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class RecargaDTO
    {
        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Moneda inválida.")]
        public int IdMoneda { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public string MetodoPago { get; set; } = null!;

        public string? NumeroReferencia { get; set; }
    }
}