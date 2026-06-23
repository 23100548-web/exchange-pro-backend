using System.ComponentModel.DataAnnotations;

namespace UESAN.ExchangePro.CORE.Core.DTOs
{
    public class PublicarOfertaDTO : IValidatableObject
    {
        [Required(ErrorMessage = "La moneda de entrega es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Moneda de entrega inválida.")]
        public int MonedaEntrega { get; set; }

        [Required(ErrorMessage = "La moneda a recibir es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Moneda a recibir inválida.")]
        public int MonedaRecibe { get; set; }

        [Required(ErrorMessage = "El monto ofertado es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto ofertado debe ser mayor a 0.")]
        public decimal MontoOfertado { get; set; }

        [Required(ErrorMessage = "El monto mínimo es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto mínimo debe ser mayor a 0.")]
        public decimal MontoMinimo { get; set; }

        [Required(ErrorMessage = "La tasa de cambio es obligatoria.")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "La tasa de cambio debe ser mayor a 0.")]
        public decimal TasaCambio { get; set; }

        [Required(ErrorMessage = "El tipo de operación es obligatorio.")]
        [RegularExpression("^(COMPRA|VENTA)$", ErrorMessage = "El tipo de operación debe ser COMPRA o VENTA.")]
        public string TipoOperacion { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MonedaEntrega == MonedaRecibe)
                yield return new ValidationResult("La moneda de entrega y la moneda a recibir no pueden ser iguales.", new[] { nameof(MonedaRecibe) });

            if (MontoMinimo > MontoOfertado)
                yield return new ValidationResult("El monto mínimo no puede ser mayor al monto ofertado.", new[] { nameof(MontoMinimo) });
        }
    }
}