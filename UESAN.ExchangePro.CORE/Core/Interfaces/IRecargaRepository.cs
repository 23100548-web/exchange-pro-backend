using UESAN.ExchangePro.CORE.Core.Entities;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface IRecargaRepository
    {
        Task<bool> Insert(Recargas recarga);
    }
}