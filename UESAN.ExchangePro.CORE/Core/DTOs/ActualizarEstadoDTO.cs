using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class ActualizarEstadoDTO
    {
        [Required(ErrorMessage = "El ID de la transacción es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID de transacción inválido.")]
        public long IdTransaccion { get; set; }

        [Required(ErrorMessage = "El nuevo estado es obligatorio.")]
        [RegularExpression("^(PAGADO|COMPLETADO|CANCELADO)$", ErrorMessage = "Estado no válido. Use PAGADO, COMPLETADO o CANCELADO.")]
        public string NuevoEstado { get; set; } = null!;
    }
}