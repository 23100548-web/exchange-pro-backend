using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class ResolucionesDisputa
{
    public long IdResolucion { get; set; }

    public long IdDisputa { get; set; }

    public long AdministradorId { get; set; }

    public string? Decision { get; set; }

    public string? Observacion { get; set; }

    public DateTime? FechaResolucion { get; set; }

    public virtual Usuarios Administrador { get; set; } = null!;

    public virtual Disputas IdDisputaNavigation { get; set; } = null!;
}
