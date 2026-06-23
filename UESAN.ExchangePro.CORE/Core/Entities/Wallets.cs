using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Wallets
{
    public long IdWallet { get; set; }

    public long IdUsuario { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosWallet> MovimientosWallet { get; set; } = new List<MovimientosWallet>();

    public virtual ICollection<WalletSaldos> WalletSaldos { get; set; } = new List<WalletSaldos>();
}
