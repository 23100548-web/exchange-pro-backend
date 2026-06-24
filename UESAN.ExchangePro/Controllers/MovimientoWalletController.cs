using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UESAN.ExchangePro.CORE.Core.DTOs;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MovimientoWalletController : ControllerBase
{
    private readonly IMovimientoWalletRepository _repo;
    private readonly IWalletRepository _walletRepo;

    public MovimientoWalletController(IMovimientoWalletRepository repo, IWalletRepository walletRepo)
    {
        _repo = repo;
        _walletRepo = walletRepo;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerMovimientos([FromQuery] int? idMoneda)
    {
        var idUsuario = long.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");

        var wallet = await _walletRepo.GetByUsuarioId(idUsuario);
        if (wallet == null)
            return NotFound(new { mensaje = "Wallet no encontrada" });

        var movimientos = await _repo.GetByWalletId(wallet.IdWallet, idMoneda);

        var result = movimientos.Select(m => new MovimientoWalletResponseDTO
        {
            IdMovimiento = m.IdMovimiento,
            MonedaCodigo = m.IdMonedaNavigation?.Codigo,
            TipoOperacion = m.TipoOperacion,
            Monto = m.Monto,
            Resultado = m.Resultado,
            ReferenciaTipo = m.ReferenciaTipo,
            ReferenciaId = m.ReferenciaId,
            FechaMovimiento = m.FechaMovimiento
        });

        return Ok(result);
    }
}