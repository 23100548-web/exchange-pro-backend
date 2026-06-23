using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Roles
{
    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
