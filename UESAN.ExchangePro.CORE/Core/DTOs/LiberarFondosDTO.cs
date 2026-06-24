using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class LiberarFondosDTO
    {
        [Required(ErrorMessage = "El ID de la transacción es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID de transacción inválido.")]
        public long IdTransaccion { get; set; }
    }
}