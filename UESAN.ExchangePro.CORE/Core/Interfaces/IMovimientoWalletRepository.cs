using UESAN.ExchangePro.CORE.Core.Entities;

public interface IMovimientoWalletRepository
{
    Task<IEnumerable<MovimientosWallet>> GetByWalletId(long idWallet, int? idMoneda = null);
    Task<bool> Insert(MovimientosWallet movimiento);
}
