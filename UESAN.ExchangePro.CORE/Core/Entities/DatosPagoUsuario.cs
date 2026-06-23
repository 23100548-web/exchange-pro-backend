using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class DatosPagoUsuario
{
    public long IdDatoPago { get; set; }

    public long IdUsuario { get; set; }

    public string? Yape { get; set; }

    public string? Plin { get; set; }

    public int? IdBanco { get; set; }

    public string? NumeroCuenta { get; set; }

    public string? Cci { get; set; }

    public virtual Bancos? IdBancoNavigation { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
