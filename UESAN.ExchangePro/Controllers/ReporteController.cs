using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Infrastructure.Data;

namespace UESAN.ExchangePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReporteController : ControllerBase
    {
        private readonly ExchangeProDbContext _context;

        public ReporteController(ExchangeProDbContext context)
        {
            _context = context;
        }

        [HttpPost("generar")]
        public async Task<IActionResult> Generar([FromBody] ReporteRequestDTO request)
        {
            var rolClaim = User.FindFirst("Rol")?.Value;
            if (rolClaim != "2")
                return Forbid();

            try
            {
                var response = await GenerateReport(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("exportar")]
        public async Task<IActionResult> Exportar([FromBody] ReporteRequestDTO request)
        {
            var rolClaim = User.FindFirst("Rol")?.Value;
            if (rolClaim != "2")
                return Forbid();

            try
            {
                var response = await GenerateExportData(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("historial")]
        public async Task<IActionResult> Historial()
        {
            var rolClaim = User.FindFirst("Rol")?.Value;
            if (rolClaim != "2")
                return Forbid();

            try
            {
                var historial = await _context.ReportesGenerados
                    .OrderByDescending(r => r.FechaGeneracion)
                    .Select(r => new
                    {
                        r.IdReporte,
                        r.TipoReporte,
                        r.FechaGeneracion,
                        r.AdministradorId
                    })
                    .ToListAsync();

                return Ok(historial);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private async Task<ReporteResponseDTO> GenerateReport(ReporteRequestDTO request)
        {
            var now = DateTime.Now;

            switch (request.Tipo.ToLower())
            {
                case "usuarios":
                    return await ReporteUsuarios(request, now);
                case "ofertas":
                    return await ReporteOfertas(request, now);
                case "transacciones":
                    return await ReporteTransacciones(request, now);
                case "recargas":
                    return await ReporteRecargas(request, now);
                case "retiros":
                    return await ReporteRetiros(request, now);
                case "disputas":
                    return await ReporteDisputas(request, now);
                default:
                    throw new ArgumentException($"Tipo de reporte '{request.Tipo}' no válido. Valores permitidos: usuarios, ofertas, transacciones, recargas, retiros, disputas.");
            }
        }

        private async Task<ReporteResponseDTO> GenerateExportData(ReporteRequestDTO request)
        {
            var report = await GenerateReport(request);

            var exportRows = report.Datos is IEnumerable<object> datos
                ? datos
                : new List<object>();

            return new ReporteResponseDTO
            {
                Tipo = request.Tipo,
                FechaGeneracion = report.FechaGeneracion,
                Resumen = report.Resumen,
                Datos = exportRows
            };
        }

        private async Task<ReporteResponseDTO> ReporteUsuarios(ReporteRequestDTO request, DateTime now)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(request.Estado))
                query = query.Where(u => u.Estado == request.Estado);

            if (request.FechaInicio.HasValue)
                query = query.Where(u => u.FechaRegistro >= request.FechaInicio.Value);

            if (request.FechaFin.HasValue)
                query = query.Where(u => u.FechaRegistro <= request.FechaFin.Value);

            var resumen = await query
                .GroupBy(u => u.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var datos = await query
                .Select(u => new
                {
                    u.IdUsuario,
                    u.NombreCompleto,
                    u.Correo,
                    u.Telefono,
                    u.DocumentoIdentidad,
                    u.Estado,
                    u.Reputacion,
                    u.FechaRegistro
                })
                .ToListAsync();

            return new ReporteResponseDTO
            {
                Tipo = "usuarios",
                FechaGeneracion = now,
                Resumen = resumen,
                Datos = datos
            };
        }

        private async Task<ReporteResponseDTO> ReporteOfertas(ReporteRequestDTO request, DateTime now)
        {
            var query = _context.Ofertas.AsQueryable();

            if (!string.IsNullOrEmpty(request.Estado))
                query = query.Where(o => o.Estado == request.Estado);

            if (request.FechaInicio.HasValue)
                query = query.Where(o => o.FechaPublicacion >= request.FechaInicio.Value);

            if (request.FechaFin.HasValue)
                query = query.Where(o => o.FechaPublicacion <= request.FechaFin.Value);

            var resumen = await query
                .GroupBy(o => new { o.TipoOperacion, o.Estado })
                .Select(g => new { g.Key.TipoOperacion, g.Key.Estado, Cantidad = g.Count() })
                .ToListAsync();

            var datos = await query
                .Select(o => new
                {
                    o.IdOferta,
                    o.IdUsuario,
                    o.TipoOperacion,
                    MonedaEntrega = o.MonedaEntregaNavigation.Codigo,
                    MonedaRecibe = o.MonedaRecibeNavigation.Codigo,
                    o.TasaCambio,
                    o.MontoOfertado,
                    o.MontoMinimo,
                    o.Estado,
                    o.FechaPublicacion
                })
                .ToListAsync();

            return new ReporteResponseDTO
            {
                Tipo = "ofertas",
                FechaGeneracion = now,
                Resumen = resumen,
                Datos = datos
            };
        }

        private async Task<ReporteResponseDTO> ReporteTransacciones(ReporteRequestDTO request, DateTime now)
        {
            var query = _context.Transacciones.AsQueryable();

            if (!string.IsNullOrEmpty(request.Estado))
                query = query.Where(t => t.Estado == request.Estado);

            if (request.FechaInicio.HasValue)
                query = query.Where(t => t.FechaInicio >= request.FechaInicio.Value);

            if (request.FechaFin.HasValue)
                query = query.Where(t => t.FechaInicio <= request.FechaFin.Value);

            var resumen = await query
                .GroupBy(t => t.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count(), TotalMonto = g.Sum(t => t.MontoOperacion) })
                .ToListAsync();

            var datos = await query
                .Select(t => new
                {
                    t.IdTransaccion,
                    t.Codigo,
                    t.IdOferta,
                    t.CompradorId,
                    t.VendedorId,
                    t.MontoOperacion,
                    t.TotalPagar,
                    t.Estado,
                    t.FechaInicio,
                    t.FechaFin
                })
                .ToListAsync();

            return new ReporteResponseDTO
            {
                Tipo = "transacciones",
                FechaGeneracion = now,
                Resumen = resumen,
                Datos = datos
            };
        }

        private async Task<ReporteResponseDTO> ReporteRecargas(ReporteRequestDTO request, DateTime now)
        {
            var query = _context.Recargas.AsQueryable();

            if (request.IdMoneda.HasValue)
                query = query.Where(r => r.IdMoneda == request.IdMoneda.Value);

            if (!string.IsNullOrEmpty(request.Estado))
                query = query.Where(r => r.Estado == request.Estado);

            if (request.FechaInicio.HasValue)
                query = query.Where(r => r.FechaRecarga >= request.FechaInicio.Value);

            if (request.FechaFin.HasValue)
                query = query.Where(r => r.FechaRecarga <= request.FechaFin.Value);

            var resumen = await query
                .GroupBy(r => new { r.IdMoneda, Moneda = r.IdMonedaNavigation.Codigo })
                .Select(g => new { g.Key.IdMoneda, g.Key.Moneda, Cantidad = g.Count(), TotalMonto = g.Sum(r => r.Monto) })
                .ToListAsync();

            var datos = await query
                .Select(r => new
                {
                    r.IdRecarga,
                    r.IdUsuario,
                    r.IdMoneda,
                    Moneda = r.IdMonedaNavigation.Codigo,
                    r.Monto,
                    r.Estado,
                    r.FechaRecarga
                })
                .ToListAsync();

            return new ReporteResponseDTO
            {
                Tipo = "recargas",
                FechaGeneracion = now,
                Resumen = resumen,
                Datos = datos
            };
        }

        private async Task<ReporteResponseDTO> ReporteRetiros(ReporteRequestDTO request, DateTime now)
        {
            var query = _context.Retiros.AsQueryable();

            if (request.IdMoneda.HasValue)
                query = query.Where(r => r.IdMoneda == request.IdMoneda.Value);

            if (!string.IsNullOrEmpty(request.Estado))
                query = query.Where(r => r.Estado == request.Estado);

            if (request.FechaInicio.HasValue)
                query = query.Where(r => r.FechaRetiro >= request.FechaInicio.Value);

            if (request.FechaFin.HasValue)
                query = query.Where(r => r.FechaRetiro <= request.FechaFin.Value);

            var resumen = await query
                .GroupBy(r => new { r.IdMoneda, Moneda = r.IdMonedaNavigation.Codigo })
                .Select(g => new { g.Key.IdMoneda, g.Key.Moneda, Cantidad = g.Count(), TotalMonto = g.Sum(r => r.Monto) })
                .ToListAsync();

            var datos = await query
                .Select(r => new
                {
                    r.IdRetiro,
                    r.IdUsuario,
                    r.IdMoneda,
                    Moneda = r.IdMonedaNavigation.Codigo,
                    r.Monto,
                    r.Estado,
                    r.FechaRetiro
                })
                .ToListAsync();

            return new ReporteResponseDTO
            {
                Tipo = "retiros",
                FechaGeneracion = now,
                Resumen = resumen,
                Datos = datos
            };
        }

        private async Task<ReporteResponseDTO> ReporteDisputas(ReporteRequestDTO request, DateTime now)
        {
            var query = _context.Disputas.AsQueryable();

            if (!string.IsNullOrEmpty(request.Estado))
                query = query.Where(d => d.Estado == request.Estado);

            if (request.FechaInicio.HasValue)
                query = query.Where(d => d.FechaCreacion >= request.FechaInicio.Value);

            if (request.FechaFin.HasValue)
                query = query.Where(d => d.FechaCreacion <= request.FechaFin.Value);

            var resumen = await query
                .GroupBy(d => d.Estado)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            var datos = await query
                .Select(d => new
                {
                    d.IdDisputa,
                    d.IdTransaccion,
                    d.UsuarioReporta,
                    d.Motivo,
                    d.Descripcion,
                    d.Estado,
                    d.FechaCreacion
                })
                .ToListAsync();

            return new ReporteResponseDTO
            {
                Tipo = "disputas",
                FechaGeneracion = now,
                Resumen = resumen,
                Datos = datos
            };
        }
    }
}
