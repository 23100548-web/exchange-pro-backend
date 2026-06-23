using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class SolicitarResetDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; } = null!;
    }
}
