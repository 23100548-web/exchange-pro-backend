using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Calificaciones
{
    public long IdCalificacion { get; set; }

    public long IdTransaccion { get; set; }

    public long UsuarioCalificador { get; set; }

    public long UsuarioCalificado { get; set; }

    public int? Puntuacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime? FechaCalificacion { get; set; }

    public virtual Transacciones IdTransaccionNavigation { get; set; } = null!;
}
