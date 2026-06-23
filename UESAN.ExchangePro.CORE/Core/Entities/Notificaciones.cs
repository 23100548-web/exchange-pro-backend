using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Notificaciones
{
    public long IdNotificacion { get; set; }

    public long? IdUsuario { get; set; }

    public string? Titulo { get; set; }

    public string? Mensaje { get; set; }

    public bool? Leido { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Usuarios? IdUsuarioNavigation { get; set; }
}
