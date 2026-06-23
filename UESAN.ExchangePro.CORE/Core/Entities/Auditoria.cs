using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Auditoria
{
    public long IdAuditoria { get; set; }

    public long? UsuarioId { get; set; }

    public string? Accion { get; set; }

    public string? TablaAfectada { get; set; }

    public long? RegistroId { get; set; }

    public DateTime? Fecha { get; set; }
}
