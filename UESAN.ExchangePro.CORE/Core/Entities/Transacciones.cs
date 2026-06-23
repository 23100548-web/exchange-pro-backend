using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Transacciones
{
    public long IdTransaccion { get; set; }

    public string? Codigo { get; set; }

    public long IdOferta { get; set; }

    public long CompradorId { get; set; }

    public long VendedorId { get; set; }

    public int IdMetodoPago { get; set; }

    public decimal? MontoOperacion { get; set; }

    public decimal? TotalPagar { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
    
    public string? RutaComprobante { get; set; }

    public virtual ICollection<Calificaciones> Calificaciones { get; set; } = new List<Calificaciones>();

    public virtual Usuarios Comprador { get; set; } = null!;

    public virtual ICollection<ComprobantesPago> ComprobantesPago { get; set; } = new List<ComprobantesPago>();

    public virtual ICollection<Disputas> Disputas { get; set; } = new List<Disputas>();

    public virtual MetodosPago IdMetodoPagoNavigation { get; set; } = null!;

    public virtual Ofertas IdOfertaNavigation { get; set; } = null!;

    public virtual ICollection<Retenciones> Retenciones { get; set; } = new List<Retenciones>();

    public virtual Usuarios Vendedor { get; set; } = null!;
}
