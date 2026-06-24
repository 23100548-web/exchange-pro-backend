using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CalificacionController : ControllerBase
{
    private readonly ICalificacionRepository _repo;
    private readonly ITransaccionRepository _transaccionRepo;
    private readonly IUsuarioRepository _usuarioRepo;

    public CalificacionController(
        ICalificacionRepository repo,
        ITransaccionRepository transaccionRepo,
        IUsuarioRepository usuarioRepo)
    {
        _repo = repo;
        _transaccionRepo = transaccionRepo;
        _usuarioRepo = usuarioRepo;
    }

    [HttpPost]
    public async Task<IActionResult> CrearCalificacion([FromBody] CrearCalificacionDTO dto)
    {
        var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");

        var yaCalifico = await _repo.ExistsByTransaccionAndUser(dto.IdTransaccion, idUsuario);
        if (yaCalifico)
            return BadRequest(new { mensaje = "Ya has calificado esta transacción." });

        var transaccion = await _transaccionRepo.GetById(dto.IdTransaccion);
        if (transaccion == null)
            return NotFound(new { mensaje = "Transacción no encontrada." });

        var usuarioCalificado = transaccion.CompradorId == idUsuario
            ? transaccion.VendedorId
            : transaccion.CompradorId;

        var calificacion = new Calificaciones
        {
            IdTransaccion = dto.IdTransaccion,
            UsuarioCalificador = idUsuario,
            UsuarioCalificado = usuarioCalificado,
            Puntuacion = dto.Puntuacion,
            Comentario = dto.Comentario,
            FechaCalificacion = DateTime.Now
        };

        if (!await _repo.Insert(calificacion))
            return StatusCode(500, new { mensaje = "Error al guardar la calificación." });

        var calificado = await _usuarioRepo.GetById(usuarioCalificado);
        if (calificado != null)
        {
            var total = calificado.TotalCalificaciones ?? 0;
            var reputacionActual = calificado.Reputacion ?? 5.00m;
            calificado.Reputacion = Math.Round((reputacionActual * total + dto.Puntuacion) / (total + 1), 2);
            calificado.TotalCalificaciones = total + 1;
            await _usuarioRepo.Update(calificado);
        }

        return Ok(new { mensaje = "Calificación registrada correctamente" });
    }

    [HttpGet("usuario/{idUsuario}")]
    public async Task<IActionResult> ObtenerCalificaciones(long idUsuario)
    {
        var lista = await _repo.GetByUsuarioCalificado(idUsuario);
        var result = lista.Select(c => new CalificacionResponseDTO
        {
            IdCalificacion = c.IdCalificacion,
            IdTransaccion = c.IdTransaccion,
            UsuarioCalificado = c.UsuarioCalificado,
            Puntuacion = c.Puntuacion ?? 0,
            Comentario = c.Comentario,
            FechaCalificacion = c.FechaCalificacion
        });
        return Ok(result);
    }

    [HttpGet("ya-califico/{idTransaccion}")]
    public async Task<IActionResult> YaCalifico(long idTransaccion)
    {
        var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
        var yaCalifico = await _repo.ExistsByTransaccionAndUser(idTransaccion, idUsuario);
        return Ok(new { yaCalifico });
    }
}