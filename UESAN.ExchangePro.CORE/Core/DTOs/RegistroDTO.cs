using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class RegistroDTO
    {
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 100 caracteres.")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres.")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        [StringLength(150)]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^\d{9,15}$", ErrorMessage = "El teléfono debe tener entre 9 y 15 dígitos.")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El documento de identidad es obligatorio.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El documento debe tener entre 8 y 20 caracteres.")]
        public string DocumentoIdentidad { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; } = null!;
    }
}