using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class ComprobantesPago
{
    public long IdComprobante { get; set; }

    public long IdTransaccion { get; set; }

    public string? NombreArchivo { get; set; }

    public string? RutaArchivo { get; set; }

    public DateTime? FechaSubida { get; set; }

    public virtual Transacciones IdTransaccionNavigation { get; set; } = null!;
}
