using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class WalletSaldos
{
    public long IdSaldo { get; set; }

    public long IdWallet { get; set; }

    public int IdMoneda { get; set; }

    public decimal? SaldoDisponible { get; set; }

    public decimal? SaldoRetenido { get; set; }

    public virtual Monedas IdMonedaNavigation { get; set; } = null!;

    public virtual Wallets IdWalletNavigation { get; set; } = null!;
}
