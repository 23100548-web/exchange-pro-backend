using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class MatchingOfertas
{
    public long IdMatching { get; set; }

    public long? OfertaBase { get; set; }

    public long? OfertaCoincidente { get; set; }

    public decimal? Compatibilidad { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Ofertas? OfertaBaseNavigation { get; set; }

    public virtual Ofertas? OfertaCoincidenteNavigation { get; set; }
}
