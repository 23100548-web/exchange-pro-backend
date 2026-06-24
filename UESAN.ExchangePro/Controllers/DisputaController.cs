using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;

namespace UESAN.ExchangePro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Todo el flujo requiere que el usuario esté logueado mediante Token JWT
    public class DisputaController : ControllerBase
    {
        private readonly IDisputaRepository _disputaRepo;
        private readonly INotificacionesRepository _notificacionesRepository;

        public DisputaController(IDisputaRepository disputaRepo, INotificacionesRepository notificacionesRepository)
        {
            _disputaRepo = disputaRepo;
            _notificacionesRepository = notificacionesRepository;
        }

        // =========================================================================
        // 1. POST: api/Disputa/abrir
        // =========================================================================
        [HttpPost("abrir")]
        public async Task<IActionResult> AbrirDisputa([FromBody] CrearDisputaDTO dto)
        {
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
            if (string.IsNullOrEmpty(idUsuarioClaim))
                return Unauthorized("Token inválido o expirado.");

            long idUsuario = long.Parse(idUsuarioClaim);

            // Armamos el objeto Disputa basado en lo que mandó el usuario
            var nuevaDisputa = new Disputas
            {
                IdTransaccion = dto.IdTransaccion,
                UsuarioReporta = idUsuario,
                Motivo = dto.Motivo,
                Descripcion = dto.Descripcion
                // Nota: El Estado ('PENDIENTE') y la Fecha se asignan automáticamente en el Repositorio
            };

            try
            {
                bool exito = await _disputaRepo.AbrirDisputa(nuevaDisputa);

                if (exito)
                {
                    await _notificacionesRepository.Create(new Notificaciones
                    {
                        IdUsuario = null,
                        Titulo = "Nueva disputa abierta",
                        Mensaje = $"El usuario {nuevaDisputa.UsuarioReporta} abrió una disputa para la transacción {nuevaDisputa.IdTransaccion}.",
                        Fecha = DateTime.UtcNow,
                        Leido = false
                    });

                    return Ok(new { mensaje = "Disputa abierta. La transacción ha sido CONGELADA en estado 'EN_DISPUTA' hasta que un administrador la revise.", idDisputa = nuevaDisputa.IdDisputa });
                }

                return BadRequest("No se pudo abrir la disputa. Verifica que la transacción sea válida.");
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR AL ABRIR DISPUTA: {ex.Message}");
            }
        }

        // =========================================================================
        // 2. POST: api/Disputa/{idDisputa}/subir-evidencia
        // =========================================================================
        [HttpPost("{idDisputa}/subir-evidencia")]
        public async Task<IActionResult> SubirEvidencia(long idDisputa, [FromForm] IFormFile archivo)
        {
            var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");

            var disputa = await _disputaRepo.GetById(idDisputa);
            if (disputa == null)
                return NotFound(new { mensaje = "Disputa no encontrada." });

            if (disputa.UsuarioReporta != idUsuario)
                return StatusCode(403, new { mensaje = "Solo el creador de la disputa puede subir evidencias." });

            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { mensaje = "Debes seleccionar un archivo." });

            var ext = Path.GetExtension(archivo.FileName).ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".pdf")
                return BadRequest(new { mensaje = "Solo se permiten archivos JPG, PNG o PDF." });

            if (archivo.Length > 10 * 1024 * 1024)
                return BadRequest(new { mensaje = "El archivo no debe superar los 10 MB." });

            string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "evidencias");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string nombre = $"disc_{idDisputa}_{Guid.NewGuid():N}{ext}";
            string rutaFisica = Path.Combine(carpeta, nombre);
            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var evidencia = new EvidenciasDisputa
            {
                IdDisputa = idDisputa,
                Archivo = $"/evidencias/{nombre}"
            };

            await _disputaRepo.InsertEvidencia(evidencia);
            return Ok(new { mensaje = "Evidencia subida correctamente.", ruta = evidencia.Archivo });
        }

        // =========================================================================
        // 3. GET: api/Disputa/pendientes
        // =========================================================================
        [HttpGet("pendientes")]
        public async Task<IActionResult> ListarDisputasPendientes()
        {
            var rolClaim = User.FindFirst("Rol")?.Value;
            if (rolClaim != "2")
            {
                return StatusCode(403, new { mensaje = "Acceso denegado. Solo un Administrador puede ver la bandeja del VAR." });
            }

            try
            {
                var disputas = await _disputaRepo.GetDisputasPendientes();

                var dtos = disputas.Select(d => new DisputaResponseDTO
                {
                    IdDisputa = d.IdDisputa,
                    IdTransaccion = d.IdTransaccion,
                    UsuarioReporta = d.UsuarioReporta,
                    UsuarioReportaNombre = d.UsuarioReportaNavigation?.NombreCompleto ?? "Usuario",
                    Motivo = d.Motivo,
                    Descripcion = d.Descripcion,
                    Estado = d.Estado,
                    FechaCreacion = d.FechaCreacion,
                    Evidencias = d.EvidenciasDisputa.Select(e => e.Archivo ?? string.Empty).ToList(),
                    TransaccionMonto = d.IdTransaccionNavigation?.MontoOperacion ?? 0,
                    TransaccionEstado = d.IdTransaccionNavigation?.Estado ?? "DESCONOCIDO",
                    CompradorNombre = d.IdTransaccionNavigation?.Comprador?.NombreCompleto ?? "Comprador",
                    VendedorNombre = d.IdTransaccionNavigation?.Vendedor?.NombreCompleto ?? "Vendedor"
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR AL LISTAR DISPUTAS PENDIENTES: {ex.Message}");
            }
        }

        // =========================================================================
        // 4. PUT: api/Disputa/resolver
        // =========================================================================
        [HttpPut("resolver")]
        public async Task<IActionResult> ResolverDisputa([FromBody] ResolverDisputaDTO dto)
        {
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
            var rolClaim = User.FindFirst("Rol")?.Value;

            if (string.IsNullOrEmpty(idUsuarioClaim))
                return Unauthorized("Token inválido o sesión expirada.");

            // BARRERA DE SEGURIDAD: Solo el Rol 2 (Admin) puede dictar sentencia
            if (rolClaim != "2")
            {
                return StatusCode(403, new { mensaje = "Acceso denegado. Solo un Administrador puede ingresar a la sala del VAR." });
            }

            long idAdmin = long.Parse(idUsuarioClaim);

            try
            {
                // Ejecutamos la lógica de resolución transaccional en la Base de Datos
                bool exito = await _disputaRepo.ResolverDisputa(dto.IdDisputa, idAdmin, dto.Decision, dto.Observacion);

                var disputa = await _disputaRepo.GetById(dto.IdDisputa);
                if (disputa == null)
                    return NotFound(new { mensaje = "Disputa no encontrada." });

                if (exito)
                {
                    await _notificacionesRepository.Create(new Notificaciones
                    {
                        IdUsuario = disputa.UsuarioReporta,
                        Titulo = "Disputa resuelta",
                        Mensaje = $"Tu disputa #{dto.IdDisputa} fue resuelta: {dto.Decision}. {dto.Observacion}",
                        Fecha = DateTime.UtcNow,
                        Leido = false
                    });

                    string msjExtra = dto.Decision.ToUpper() == "A_FAVOR_COMPRADOR"
                        ? "Fondos liberados a la Wallet del comprador."
                        : "Transacción anulada y stock de la oferta devuelto al mercado P2P.";

                    return Ok(new { mensaje = $"Fallo emitido exitosamente por el VAR. {msjExtra}" });
                }

                return BadRequest("No se pudo procesar la resolución. Verifica que el ID de la disputa sea correcto.");
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR CRÍTICO EN LA SALA DEL VAR: {ex.Message}");
            }
        }
    }
}