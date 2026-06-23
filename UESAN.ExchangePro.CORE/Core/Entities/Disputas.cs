using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Disputas
{
    public long IdDisputa { get; set; }

    public long IdTransaccion { get; set; }

    public long UsuarioReporta { get; set; }

    public string? Motivo { get; set; }

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<EvidenciasDisputa> EvidenciasDisputa { get; set; } = new List<EvidenciasDisputa>();

    public virtual Transacciones IdTransaccionNavigation { get; set; } = null!;

    public virtual ICollection<ResolucionesDisputa> ResolucionesDisputa { get; set; } = new List<ResolucionesDisputa>();

    public virtual Usuarios UsuarioReportaNavigation { get; set; } = null!;
}
