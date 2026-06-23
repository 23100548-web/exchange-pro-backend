using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Recargas
{
    public long IdRecarga { get; set; }

    public long IdUsuario { get; set; }

    public int IdMoneda { get; set; }

    public decimal Monto { get; set; }

    public string? MetodoPago { get; set; }

    public string? NumeroReferencia { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaRecarga { get; set; }

    public virtual Monedas IdMonedaNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
