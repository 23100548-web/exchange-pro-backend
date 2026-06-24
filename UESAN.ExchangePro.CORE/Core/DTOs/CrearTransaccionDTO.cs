using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class CrearTransaccionDTO
    {
        [Required(ErrorMessage = "El ID de la oferta es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de oferta inválido.")]
        public int IdOferta { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Método de pago inválido.")]
        public int IdMetodoPago { get; set; }

        [Required(ErrorMessage = "El monto a operar es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }
    }
}