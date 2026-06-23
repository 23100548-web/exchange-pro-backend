using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class MetodosPago
{
    public int IdMetodoPago { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Transacciones> Transacciones { get; set; } = new List<Transacciones>();

    public virtual ICollection<Ofertas> IdOferta { get; set; } = new List<Ofertas>();
}
