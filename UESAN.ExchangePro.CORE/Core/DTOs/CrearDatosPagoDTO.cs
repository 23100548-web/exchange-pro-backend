using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class CrearDatosPagoDTO : IValidatableObject
    {
        [StringLength(20, ErrorMessage = "Yape debe tener máximo 20 caracteres.")]
        public string? Yape { get; set; }

        [StringLength(20, ErrorMessage = "Plin debe tener máximo 20 caracteres.")]
        public string? Plin { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Banco inválido.")]
        public int? IdBanco { get; set; }

        [StringLength(30, ErrorMessage = "Número de cuenta debe tener máximo 30 caracteres.")]
        public string? NumeroCuenta { get; set; }

        [StringLength(30, ErrorMessage = "CCI debe tener máximo 30 caracteres.")]
        public string? Cci { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool tieneYape = !string.IsNullOrWhiteSpace(Yape);
            bool tienePlin = !string.IsNullOrWhiteSpace(Plin);
            bool tieneBanco = IdBanco.HasValue && !string.IsNullOrWhiteSpace(NumeroCuenta);

            if (!tieneYape && !tienePlin && !tieneBanco)
                yield return new ValidationResult("Debe proporcionar al menos un método de pago: Yape, Plin o datos bancarios.");
        }
    }
}