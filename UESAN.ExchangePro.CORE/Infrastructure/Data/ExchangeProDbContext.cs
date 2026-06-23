using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Entities;

namespace UESAN.ExchangePro.CORE.Infrastructure.Data;

public partial class ExchangeProDbContext : DbContext
{
    public ExchangeProDbContext()
    {
    }

    public ExchangeProDbContext(DbContextOptions<ExchangeProDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditoria> Auditoria { get; set; }

    public virtual DbSet<Bancos> Bancos { get; set; }

    public virtual DbSet<Calificaciones> Calificaciones { get; set; }

    public virtual DbSet<ComprobantesPago> ComprobantesPago { get; set; }

    public virtual DbSet<DatosPagoUsuario> DatosPagoUsuario { get; set; }

    public virtual DbSet<Disputas> Disputas { get; set; }

    public virtual DbSet<EvidenciasDisputa> EvidenciasDisputa { get; set; }

    public virtual DbSet<MatchingOfertas> MatchingOfertas { get; set; }

    public virtual DbSet<MetodosPago> MetodosPago { get; set; }

    public virtual DbSet<Monedas> Monedas { get; set; }

    public virtual DbSet<MovimientosWallet> MovimientosWallet { get; set; }

    public virtual DbSet<Notificaciones> Notificaciones { get; set; }

    public virtual DbSet<Ofertas> Ofertas { get; set; }

    public virtual DbSet<Recargas> Recargas { get; set; }

    public virtual DbSet<ReportesGenerados> ReportesGenerados { get; set; }

    public virtual DbSet<ResolucionesDisputa> ResolucionesDisputa { get; set; }

    public virtual DbSet<Retenciones> Retenciones { get; set; }

    public virtual DbSet<Retiros> Retiros { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Transacciones> Transacciones { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<WalletSaldos> WalletSaldos { get; set; }

    public virtual DbSet<Wallets> Wallets { get; set; }

    public virtual DbSet<Feedback> Feedback { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__7FD13FA0A62BDA1D");

            entity.Property(e => e.Accion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bancos>(entity =>
        {
            entity.HasKey(e => e.IdBanco).HasName("PK__Bancos__2D3F553EC92ADCCA");

            entity.HasIndex(e => e.Nombre, "UQ__Bancos__75E3EFCFDF2595EB").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Calificaciones>(entity =>
        {
            entity.HasKey(e => e.IdCalificacion).HasName("PK__Califica__40E4A75108201679");

            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaCalificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdTransaccionNavigation).WithMany(p => p.Calificaciones)
                .HasForeignKey(d => d.IdTransaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calificac__IdTra__06CD04F7");
        });

        modelBuilder.Entity<ComprobantesPago>(entity =>
        {
            entity.HasKey(e => e.IdComprobante).HasName("PK__Comproba__BF4686EDCE0B70BF");

            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreArchivo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RutaArchivo)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTransaccionNavigation).WithMany(p => p.ComprobantesPago)
                .HasForeignKey(d => d.IdTransaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comproban__IdTra__74AE54BC");
        });

        modelBuilder.Entity<DatosPagoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdDatoPago).HasName("PK__DatosPag__6C877D30725AA46F");

            entity.Property(e => e.Cci)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("CCI");
            entity.Property(e => e.NumeroCuenta)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Plin)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Yape)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdBancoNavigation).WithMany(p => p.DatosPagoUsuario)
                .HasForeignKey(d => d.IdBanco)
                .HasConstraintName("FK__DatosPago__IdBan__44FF419A");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.DatosPagoUsuario)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatosPago__IdUsu__440B1D61");
        });

        modelBuilder.Entity<Disputas>(entity =>
        {
            entity.HasKey(e => e.IdDisputa).HasName("PK__Disputas__58C8CAE47AA4895A");

            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("ABIERTA");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Motivo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTransaccionNavigation).WithMany(p => p.Disputas)
                .HasForeignKey(d => d.IdTransaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Disputas__IdTran__797309D9");

            entity.HasOne(d => d.UsuarioReportaNavigation).WithMany(p => p.Disputas)
                .HasForeignKey(d => d.UsuarioReporta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Disputas__Usuari__7A672E12");
        });

        modelBuilder.Entity<EvidenciasDisputa>(entity =>
        {
            entity.HasKey(e => e.IdEvidencia).HasName("PK__Evidenci__C602EF7E22B14B24");

            entity.Property(e => e.Archivo)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDisputaNavigation).WithMany(p => p.EvidenciasDisputa)
                .HasForeignKey(d => d.IdDisputa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Evidencia__IdDis__7D439ABD");
        });

        modelBuilder.Entity<MatchingOfertas>(entity =>
        {
            entity.HasKey(e => e.IdMatching).HasName("PK__Matching__45228EAFF07A42A5");

            entity.Property(e => e.Compatibilidad).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.OfertaBaseNavigation).WithMany(p => p.MatchingOfertasOfertaBaseNavigation)
                .HasForeignKey(d => d.OfertaBase)
                .HasConstraintName("FK__MatchingO__Ofert__5FB337D6");

            entity.HasOne(d => d.OfertaCoincidenteNavigation).WithMany(p => p.MatchingOfertasOfertaCoincidenteNavigation)
                .HasForeignKey(d => d.OfertaCoincidente)
                .HasConstraintName("FK__MatchingO__Ofert__60A75C0F");
        });

        modelBuilder.Entity<MetodosPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodosP__6F49A9BECB2F44BE");

            entity.HasIndex(e => e.Nombre, "UQ__MetodosP__75E3EFCF7734A161").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Monedas>(entity =>
        {
            entity.HasKey(e => e.IdMoneda).HasName("PK__Monedas__AA690671CF2D330E");

            entity.HasIndex(e => e.Codigo, "UQ__Monedas__06370DACA31DD5E3").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MovimientosWallet>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PK__Movimien__881A6AE023101D9F");

            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReferenciaTipo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Resultado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdMonedaNavigation).WithMany(p => p.MovimientosWallet)
                .HasForeignKey(d => d.IdMoneda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movimient__IdMon__70DDC3D8");

            entity.HasOne(d => d.IdWalletNavigation).WithMany(p => p.MovimientosWallet)
                .HasForeignKey(d => d.IdWallet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movimient__IdWal__6FE99F9F");
        });

        modelBuilder.Entity<Notificaciones>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__Notifica__F6CA0A85F1903ACA");

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Leido).HasDefaultValue(false);
            entity.Property(e => e.Mensaje).IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Notificac__IdUsu__0E6E26BF");
        });

        modelBuilder.Entity<Ofertas>(entity =>
        {
            entity.HasKey(e => e.IdOferta).HasName("PK__Ofertas__5420E1DA4CB123D0");

            entity.HasIndex(e => e.Estado, "IX_Ofertas_Estado");

            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVA");
            entity.Property(e => e.FechaPublicacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoMinimo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MontoOfertado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TasaCambio).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.TipoOperacion)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ofertas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ofertas__IdUsuar__5629CD9C");

            entity.HasOne(d => d.MonedaEntregaNavigation).WithMany(p => p.OfertasMonedaEntregaNavigation)
                .HasForeignKey(d => d.MonedaEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ofertas__MonedaE__571DF1D5");

            entity.HasOne(d => d.MonedaRecibeNavigation).WithMany(p => p.OfertasMonedaRecibeNavigation)
                .HasForeignKey(d => d.MonedaRecibe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ofertas__MonedaR__5812160E");

            entity.HasMany(d => d.IdMetodoPago).WithMany(p => p.IdOferta)
                .UsingEntity<Dictionary<string, object>>(
                    "OfertaMetodoPago",
                    r => r.HasOne<MetodosPago>().WithMany()
                        .HasForeignKey("IdMetodoPago")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__OfertaMet__IdMet__5BE2A6F2"),
                    l => l.HasOne<Ofertas>().WithMany()
                        .HasForeignKey("IdOferta")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__OfertaMet__IdOfe__5AEE82B9"),
                    j =>
                    {
                        j.HasKey("IdOferta", "IdMetodoPago").HasName("PK__OfertaMe__A2D47B41E3F6E1DF");
                    });
        });

        modelBuilder.Entity<Recargas>(entity =>
        {
            entity.HasKey(e => e.IdRecarga).HasName("PK__Recargas__6BD180D4A00FB760");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("COMPLETADA");
            entity.Property(e => e.FechaRecarga)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumeroReferencia)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdMonedaNavigation).WithMany(p => p.Recargas)
                .HasForeignKey(d => d.IdMoneda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recargas__IdMone__4AB81AF0");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Recargas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recargas__IdUsua__49C3F6B7");
        });

        modelBuilder.Entity<ReportesGenerados>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PK__Reportes__F95611369BAF2CA6");

            entity.Property(e => e.FechaGeneracion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoReporte)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Administrador).WithMany(p => p.ReportesGenerados)
                .HasForeignKey(d => d.AdministradorId)
                .HasConstraintName("FK_ReportesGenerados_Usuarios");
        });

        modelBuilder.Entity<ResolucionesDisputa>(entity =>
        {
            entity.HasKey(e => e.IdResolucion).HasName("PK__Resoluci__18BB5C9F0BF1306E");

            entity.Property(e => e.Decision)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaResolucion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Observacion).IsUnicode(false);

            entity.HasOne(d => d.Administrador).WithMany(p => p.ResolucionesDisputa)
                .HasForeignKey(d => d.AdministradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resolucio__Admin__02084FDA");

            entity.HasOne(d => d.IdDisputaNavigation).WithMany(p => p.ResolucionesDisputa)
                .HasForeignKey(d => d.IdDisputa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resolucio__IdDis__01142BA1");
        });

        modelBuilder.Entity<Retenciones>(entity =>
        {
            entity.HasKey(e => e.IdRetencion).HasName("PK__Retencio__CD33D72215E61F11");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.Retenciones)
                .HasForeignKey(d => d.IdOferta)
                .HasConstraintName("FK_Retenciones_Oferta");

            entity.HasOne(d => d.IdTransaccionNavigation).WithMany(p => p.Retenciones)
                .HasForeignKey(d => d.IdTransaccion)
                .HasConstraintName("FK_Retenciones_Transaccion");
        });

        modelBuilder.Entity<Retiros>(entity =>
        {
            entity.HasKey(e => e.IdRetiro).HasName("PK__Retiros__A0F85CFDC2815777");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("COMPLETADO");
            entity.Property(e => e.FechaRetiro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoRetiro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CuentaDestino)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Titular)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdMonedaNavigation).WithMany(p => p.Retiros)
                .HasForeignKey(d => d.IdMoneda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retiros__IdMoned__5070F446");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Retiros)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Retiros__IdUsuar__4F7CD00D");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C3C33277F");

            entity.HasIndex(e => e.Nombre, "UQ__Roles__75E3EFCF02EA499D").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transacciones>(entity =>
        {
            entity.HasKey(e => e.IdTransaccion).HasName("PK__Transacc__334B1F7703126014");

            entity.HasIndex(e => e.Estado, "IX_Transacciones_Estado");

            entity.HasIndex(e => e.Codigo, "UQ__Transacc__06370DAC928237E6").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasDefaultValue("PENDIENTE_PAGO");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoOperacion).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPagar).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Comprador).WithMany(p => p.TransaccionesComprador)
                .HasForeignKey(d => d.CompradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacci__Compr__6754599E");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacci__IdMet__693CA210");

            entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdOferta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacci__IdOfe__66603565");

            entity.HasOne(d => d.Vendedor).WithMany(p => p.TransaccionesVendedor)
                .HasForeignKey(d => d.VendedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacci__Vende__68487DD7");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97FDFBF922");

            entity.HasIndex(e => e.Correo, "IX_Usuarios_Correo");

            entity.HasIndex(e => e.DocumentoIdentidad, "UQ__Usuarios__049E81A92EC8C681").IsUnique();

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A196A38254E").IsUnique();

            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.DocumentoIdentidad)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVO");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasComputedColumnSql("([Nombres]+' '+[Apellidos])")
                .IsRequired(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Reputacion)
                .HasDefaultValue(5.00m)
                .HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalCalificaciones).HasDefaultValue(0);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__IdRol__2D27B809");
        });

        modelBuilder.Entity<WalletSaldos>(entity =>
        {
            entity.HasKey(e => e.IdSaldo).HasName("PK__WalletSa__0EAE8D65C6EE17B5");

            entity.HasIndex(e => new { e.IdWallet, e.IdMoneda }, "UQ__WalletSa__38BD6113A99F4028").IsUnique();

            entity.Property(e => e.SaldoDisponible)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SaldoRetenido)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdMonedaNavigation).WithMany(p => p.WalletSaldos)
                .HasForeignKey(d => d.IdMoneda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WalletSal__IdMon__412EB0B6");

            entity.HasOne(d => d.IdWalletNavigation).WithMany(p => p.WalletSaldos)
                .HasForeignKey(d => d.IdWallet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WalletSal__IdWal__403A8C7D");
        });

        modelBuilder.Entity<Wallets>(entity =>
        {
            entity.HasKey(e => e.IdWallet).HasName("PK__Wallets__321BF175C2A26228");

            entity.HasIndex(e => e.IdUsuario, "UQ__Wallets__5B65BF9690EE089D").IsUnique();

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Wallets)
                .HasForeignKey<Wallets>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Wallets__IdUsuar__3A81B327");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.IdFeedback);

            entity.ToTable("Feedback");

            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(2000)
                .IsRequired();

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("PENDIENTE");

            entity.Property(e => e.RespuestaAdmin)
                .HasMaxLength(2000);

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.Feedback)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedback_Usuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}