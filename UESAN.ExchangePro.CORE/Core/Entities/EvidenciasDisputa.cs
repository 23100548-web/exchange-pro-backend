using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class EvidenciasDisputa
{
    public long IdEvidencia { get; set; }

    public long IdDisputa { get; set; }

    public string? Archivo { get; set; }

    public virtual Disputas IdDisputaNavigation { get; set; } = null!;
}
