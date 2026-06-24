using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;

namespace UESAN.ExchangePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly INotificacionesRepository _notificacionesRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository, INotificacionesRepository notificacionesRepository)
        {
            _feedbackRepository = feedbackRepository;
            _notificacionesRepository = notificacionesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackDTO feedbackDTO)
        {
            try
            {
                var userIdClaim = User.FindFirst("IdUsuario")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { error = "Usuario no autenticado" });

                long idUsuario = long.Parse(userIdClaim);

                var feedback = new Feedback
                {
                    IdUsuario = idUsuario,
                    Tipo = feedbackDTO.Tipo,
                    Titulo = feedbackDTO.Titulo,
                    Descripcion = feedbackDTO.Descripcion,
                    FechaCreacion = DateTime.UtcNow,
                    Estado = "PENDIENTE"
                };

                bool result = await _feedbackRepository.Insert(feedback);
                if (result)
                {
                    await _notificacionesRepository.Create(new Notificaciones
                    {
                        IdUsuario = null,
                        Titulo = "Nuevo feedback recibido",
                        Mensaje = $"Se ha recibido un feedback de tipo '{feedback.Tipo}'.",
                        Fecha = DateTime.UtcNow,
                        Leido = false
                    });

                    return Ok(new { mensaje = "Feedback enviado correctamente. ¡Gracias por tu aporte!" });
                }

                return BadRequest(new { error = "No se pudo enviar el feedback" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var rolClaim = User.FindFirst("Rol")?.Value;
                if (rolClaim != "2")
                    return Forbid();

                var feedbacks = await _feedbackRepository.GetAll();

                var feedbackDTOs = feedbacks.Select(f => new FeedbackResponseDTO
                {
                    IdFeedback = f.IdFeedback,
                    Tipo = f.Tipo,
                    Titulo = f.Titulo,
                    Descripcion = f.Descripcion,
                    FechaCreacion = f.FechaCreacion,
                    Estado = f.Estado,
                    RespuestaAdmin = f.RespuestaAdmin,
                    IdUsuario = f.IdUsuario,
                    UsuarioNombre = f.IdUsuarioNavigation?.NombreCompleto ?? f.IdUsuarioNavigation?.Correo ?? "Usuario",
                    UsuarioEmail = f.IdUsuarioNavigation?.Correo ?? string.Empty
                }).ToList();

                return Ok(feedbackDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}/responder")]
        public async Task<IActionResult> Responder(long id, [FromBody] ResponderFeedbackDTO responderDTO)
        {
            try
            {
                var rolClaim = User.FindFirst("Rol")?.Value;
                if (rolClaim != "2")
                    return Forbid();

                var feedback = await _feedbackRepository.GetById(id);
                if (feedback == null)
                    return NotFound(new { error = "Feedback no encontrado" });

                feedback.RespuestaAdmin = responderDTO.Respuesta;
                feedback.Estado = "REVISADO";

                bool result = await _feedbackRepository.Update(feedback);
                if (result)
                {
                    await _notificacionesRepository.Create(new Notificaciones
                    {
                        IdUsuario = feedback.IdUsuario,
                        Titulo = "Tu feedback ha sido respondido",
                        Mensaje = $"El administrador respondió tu feedback: {responderDTO.Respuesta}",
                        Fecha = DateTime.UtcNow,
                        Leido = false
                    });

                    return Ok(new { mensaje = "Feedback respondido correctamente" });
                }

                return BadRequest(new { error = "No se pudo responder el feedback" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class CreateFeedbackDTO
    {
        public string Tipo { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }

    public class ResponderFeedbackDTO
    {
        public string Respuesta { get; set; } = null!;
    }
}