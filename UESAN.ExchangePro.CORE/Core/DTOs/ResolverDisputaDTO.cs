using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class ResolverDisputaDTO
    {
        [Required(ErrorMessage = "El ID de la disputa es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "ID de disputa inválido.")]
        public long IdDisputa { get; set; }

        [Required(ErrorMessage = "La decisión es obligatoria.")]
        [RegularExpression("^(A_FAVOR_COMPRADOR|A_FAVOR_VENDEDOR)$", ErrorMessage = "La decisión debe ser A_FAVOR_COMPRADOR o A_FAVOR_VENDEDOR.")]
        public string Decision { get; set; } = null!;

        [Required(ErrorMessage = "La observación es obligatoria.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "La observación debe tener entre 10 y 500 caracteres.")]
        public string Observacion { get; set; } = null!;
    }
}