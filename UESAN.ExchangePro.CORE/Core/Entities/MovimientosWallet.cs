using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class MovimientosWallet
{
    public long IdMovimiento { get; set; }

    public long IdWallet { get; set; }

    public int IdMoneda { get; set; }

    public string? TipoOperacion { get; set; }

    public decimal? Monto { get; set; }

    public string? Resultado { get; set; }

    public string? ReferenciaTipo { get; set; }

    public long? ReferenciaId { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public virtual Monedas IdMonedaNavigation { get; set; } = null!;

    public virtual Wallets IdWalletNavigation { get; set; } = null!;
}
