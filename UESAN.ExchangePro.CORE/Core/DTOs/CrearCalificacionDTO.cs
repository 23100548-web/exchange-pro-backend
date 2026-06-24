using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class CrearCalificacionDTO
    {
        public long IdTransaccion { get; set; }

        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
        public int Puntuacion { get; set; }

        [StringLength(500, ErrorMessage = "El comentario debe tener máximo 500 caracteres.")]
        public string? Comentario { get; set; }
    }
}