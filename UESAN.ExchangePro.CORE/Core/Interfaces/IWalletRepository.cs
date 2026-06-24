using UESAN.ExchangePro.CORE.Core.Entities;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallets?> GetByUsuarioId(long idUsuario);
        Task<bool> Update(Wallets wallet);
        Task<bool> Insert(Wallets wallet);
    }
}