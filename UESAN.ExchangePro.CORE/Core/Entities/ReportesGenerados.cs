using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class ReportesGenerados
{
    public long IdReporte { get; set; }

    public long? AdministradorId { get; set; }

    public string? TipoReporte { get; set; }

    public DateTime? FechaGeneracion { get; set; }

    public virtual Usuarios? Administrador { get; set; }
}
