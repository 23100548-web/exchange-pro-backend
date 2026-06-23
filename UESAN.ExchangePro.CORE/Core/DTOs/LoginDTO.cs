using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = null!;
    }
}