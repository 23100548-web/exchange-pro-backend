using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Bancos
{
    public int IdBanco { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<DatosPagoUsuario> DatosPagoUsuario { get; set; } = new List<DatosPagoUsuario>();
}
