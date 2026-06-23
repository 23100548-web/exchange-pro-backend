using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class RestablecerPasswordDTO
    {
        [Required(ErrorMessage = "El token es obligatorio.")]
        public string ResetToken { get; set; } = null!;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        public string NuevaPassword { get; set; } = null!;

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; } = null!;
    }
}
