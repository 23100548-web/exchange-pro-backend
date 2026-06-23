using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Retiros
{
    public long IdRetiro { get; set; }

    public long IdUsuario { get; set; }

    public int IdMoneda { get; set; }

    public decimal Monto { get; set; }

    public string? MetodoRetiro { get; set; }

    public string? CuentaDestino { get; set; }

    public string? Titular { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaRetiro { get; set; }

    public virtual Monedas IdMonedaNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
