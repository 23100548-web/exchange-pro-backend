using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Entities;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DatosPagoController : ControllerBase
{
    private readonly IDatosPagoRepository _repo;
    public DatosPagoController(IDatosPagoRepository repo) => _repo = repo;

    [HttpGet("{idUsuario}")]
    public async Task<IActionResult> ObtenerDatosPago(long idUsuario)
    {
        var datos = await _repo.GetByUsuario(idUsuario);
        var result = datos.Select(d => new DatosPagoResponseDTO
        {
            IdDatoPago = d.IdDatoPago,
            Yape = d.Yape,
            Plin = d.Plin,
            IdBanco = d.IdBanco,
            NumeroCuenta = d.NumeroCuenta,
            Cci = d.Cci,
            BancoNombre = d.IdBancoNavigation?.Nombre
        });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AgregarDatoPago([FromBody] CrearDatosPagoDTO dto)
    {
        var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");

        var datos = new DatosPagoUsuario
        {
            IdUsuario = idUsuario,
            Yape = dto.Yape,
            Plin = dto.Plin,
            IdBanco = dto.IdBanco,
            NumeroCuenta = dto.NumeroCuenta,
            Cci = dto.Cci
        };

        await _repo.Insert(datos);
        return Ok(new { mensaje = "Método de pago agregado correctamente" });
    }
}