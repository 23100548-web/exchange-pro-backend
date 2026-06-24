using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;

namespace UESAN.ExchangePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionesRepository _notificacionesRepository;

        public NotificacionesController(INotificacionesRepository notificacionesRepository)
        {
            _notificacionesRepository = notificacionesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
            if (string.IsNullOrEmpty(idUsuarioClaim))
                return Unauthorized();

            var rolClaim = User.FindFirst("Rol")?.Value;
            bool esAdmin = rolClaim == "2";
            long idUsuario = long.Parse(idUsuarioClaim);

            var notificaciones = await _notificacionesRepository.GetForUsuario(idUsuario, esAdmin);
            return Ok(notificaciones);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
            if (string.IsNullOrEmpty(idUsuarioClaim))
                return Unauthorized();

            var rolClaim = User.FindFirst("Rol")?.Value;
            bool esAdmin = rolClaim == "2";
            long idUsuario = long.Parse(idUsuarioClaim);

            var count = await _notificacionesRepository.GetUnreadCount(idUsuario, esAdmin);
            return Ok(new { unreadCount = count });
        }

        [HttpPut("{id}/leer")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var idUsuarioClaim = User.FindFirst("IdUsuario")?.Value;
            if (string.IsNullOrEmpty(idUsuarioClaim))
                return Unauthorized();

            var rolClaim = User.FindFirst("Rol")?.Value;
            bool esAdmin = rolClaim == "2";
            long idUsuario = long.Parse(idUsuarioClaim);

            bool result = await _notificacionesRepository.MarkAsRead(id, idUsuario, esAdmin);
            if (!result)
                return BadRequest(new { error = "No se encontró la notificación o no tienes permiso" });

            return Ok(new { mensaje = "Notificación marcada como leída" });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notificaciones notificacion)
        {
            if (notificacion == null) return BadRequest(new { error = "Notificación inválida" });

            bool result = await _notificacionesRepository.Create(notificacion);
            if (result)
                return Ok(new { mensaje = "Notificación creada" });

            return BadRequest(new { error = "No se pudo crear la notificación" });
        }
    }
}
