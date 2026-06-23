using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Retenciones
{
    public long IdRetencion { get; set; }

    public long? IdOferta { get; set; }

    public long? IdTransaccion { get; set; }

    public decimal? Monto { get; set; }

    public string? Estado { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Ofertas? IdOfertaNavigation { get; set; }

    public virtual Transacciones? IdTransaccionNavigation { get; set; }
}
