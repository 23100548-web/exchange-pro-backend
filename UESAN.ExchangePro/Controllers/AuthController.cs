using Microsoft.AspNetCore.Mvc;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Interfaces;
using System;

namespace UESAN.ExchangePro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroDTO registroDTO)
        {
            try
            {
                bool resultado = await _authService.Registrar(registroDTO);
                if (resultado)
                    return Ok(new { mensaje = "Usuario registrado y Wallet creada con éxito." });

                return BadRequest("No se pudo registrar el usuario.");
            }
            catch (Exception ex)
            {
                
                var mensaje = ex.Message;
                if (ex.InnerException != null)
                    mensaje += " | Detalle: " + ex.InnerException.Message;

                return BadRequest(new { error = mensaje });
            }
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                string token = await _authService.Login(loginDTO);
                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("solicitar-reset")]
        public async Task<IActionResult> SolicitarReset([FromBody] SolicitarResetDTO dto)
        {
            try
            {
                await _authService.SolicitarReset(dto);
                return Ok(new { mensaje = "Si el correo está registrado, recibirás un enlace de recuperación." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("restablecer-password")]
        public async Task<IActionResult> RestablecerPassword([FromBody] RestablecerPasswordDTO dto)
        {
            try
            {
                bool exito = await _authService.RestablecerPassword(dto);
                if (exito)
                    return Ok(new { mensaje = "Contraseña restablecida correctamente." });
                return BadRequest(new { error = "No se pudo restablecer la contraseña." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}