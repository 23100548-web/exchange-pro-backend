using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Interfaces;

namespace UESAN.ExchangePro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PerfilController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepo;

        public PerfilController(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPerfil()
        {
            var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            var usuario = await _usuarioRepo.GetById(idUsuario);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(new PerfilResponseDTO
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                DocumentoIdentidad = usuario.DocumentoIdentidad,
                FotoPerfil = usuario.FotoPerfil,
                Reputacion = usuario.Reputacion,
                TotalCalificaciones = usuario.TotalCalificaciones
            });
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarPerfil([FromBody] ActualizarPerfilDTO dto)
        {
            var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            var usuario = await _usuarioRepo.GetById(idUsuario);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            if (!string.IsNullOrWhiteSpace(dto.Nombres))
                usuario.Nombres = dto.Nombres;

            if (!string.IsNullOrWhiteSpace(dto.Apellidos))
                usuario.Apellidos = dto.Apellidos;

            if (!string.IsNullOrWhiteSpace(dto.Telefono))
                usuario.Telefono = dto.Telefono;

            if (!string.IsNullOrWhiteSpace(dto.FotoPerfil))
                usuario.FotoPerfil = dto.FotoPerfil;

            var exito = await _usuarioRepo.Update(usuario);

            if (!exito)
                return BadRequest(new { mensaje = "No se pudo actualizar el perfil." });

            return Ok(new { mensaje = "Perfil actualizado correctamente." });
        }

        [HttpPost("subir-foto")]
        public async Task<IActionResult> SubirFoto([FromForm] IFormFile foto)
        {
            var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");

            if (foto == null || foto.Length == 0)
                return BadRequest(new { mensaje = "Debes seleccionar una imagen." });

            var usuario = await _usuarioRepo.GetById(idUsuario);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotos");

            if (!Directory.Exists(carpetaDestino))
                Directory.CreateDirectory(carpetaDestino);

            string extension = Path.GetExtension(foto.FileName);
            string nombreArchivo = $"{idUsuario}_{Guid.NewGuid():N}{extension}";
            string rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }

            string rutaRelativa = $"/fotos/{nombreArchivo}";

            usuario.FotoPerfil = rutaRelativa;
            await _usuarioRepo.Update(usuario);

            return Ok(new { ruta = rutaRelativa });
        }
    }
}
