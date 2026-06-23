using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Ofertas
{
    public long IdOferta { get; set; }

    public long IdUsuario { get; set; }

    public string? TipoOperacion { get; set; }

    public int MonedaEntrega { get; set; }

    public int MonedaRecibe { get; set; }

    public decimal TasaCambio { get; set; }

    public decimal MontoOfertado { get; set; }

    public decimal MontoMinimo { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<MatchingOfertas> MatchingOfertasOfertaBaseNavigation { get; set; } = new List<MatchingOfertas>();

    public virtual ICollection<MatchingOfertas> MatchingOfertasOfertaCoincidenteNavigation { get; set; } = new List<MatchingOfertas>();

    public virtual Monedas MonedaEntregaNavigation { get; set; } = null!;

    public virtual Monedas MonedaRecibeNavigation { get; set; } = null!;

    public virtual ICollection<Retenciones> Retenciones { get; set; } = new List<Retenciones>();

    public virtual ICollection<Transacciones> Transacciones { get; set; } = new List<Transacciones>();

    public virtual ICollection<MetodosPago> IdMetodoPago { get; set; } = new List<MetodosPago>();
}
