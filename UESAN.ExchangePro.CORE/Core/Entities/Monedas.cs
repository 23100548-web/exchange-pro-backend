using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Monedas
{
    public int IdMoneda { get; set; }

    public string? Codigo { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<MovimientosWallet> MovimientosWallet { get; set; } = new List<MovimientosWallet>();

    public virtual ICollection<Ofertas> OfertasMonedaEntregaNavigation { get; set; } = new List<Ofertas>();

    public virtual ICollection<Ofertas> OfertasMonedaRecibeNavigation { get; set; } = new List<Ofertas>();

    public virtual ICollection<Recargas> Recargas { get; set; } = new List<Recargas>();

    public virtual ICollection<Retiros> Retiros { get; set; } = new List<Retiros>();

    public virtual ICollection<WalletSaldos> WalletSaldos { get; set; } = new List<WalletSaldos>();
}
