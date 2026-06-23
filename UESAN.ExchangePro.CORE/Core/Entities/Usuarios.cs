using System;
using System.Collections.Generic;

namespace UESAN.ExchangePro.CORE.Core.Entities;

public partial class Usuarios
{
    public long IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? NombreCompleto { get; set; }

    public string Correo { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string DocumentoIdentidad { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public decimal? Reputacion { get; set; }

    public int? TotalCalificaciones { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public string? FotoPerfil { get; set; }

    public virtual ICollection<DatosPagoUsuario> DatosPagoUsuario { get; set; } = new List<DatosPagoUsuario>();

    public virtual ICollection<Disputas> Disputas { get; set; } = new List<Disputas>();

    public virtual ICollection<Feedback> Feedback { get; set; } = new List<Feedback>();

    public virtual Roles IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();

    public virtual ICollection<Ofertas> Ofertas { get; set; } = new List<Ofertas>();

    public virtual ICollection<Recargas> Recargas { get; set; } = new List<Recargas>();

    public virtual ICollection<ReportesGenerados> ReportesGenerados { get; set; } = new List<ReportesGenerados>();

    public virtual ICollection<ResolucionesDisputa> ResolucionesDisputa { get; set; } = new List<ResolucionesDisputa>();

    public virtual ICollection<Retiros> Retiros { get; set; } = new List<Retiros>();

    public virtual ICollection<Transacciones> TransaccionesComprador { get; set; } = new List<Transacciones>();

    public virtual ICollection<Transacciones> TransaccionesVendedor { get; set; } = new List<Transacciones>();

    public virtual Wallets? Wallets { get; set; }
}
