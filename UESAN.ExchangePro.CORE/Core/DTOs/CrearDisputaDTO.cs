using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class CrearDisputaDTO
    {
        [Required(ErrorMessage = "El ID de la transacción es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID de transacción inválido.")]
        public long IdTransaccion { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 100 caracteres.")]
        public string Motivo { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 1000 caracteres.")]
        public string Descripcion { get; set; } = null!;
    }
}