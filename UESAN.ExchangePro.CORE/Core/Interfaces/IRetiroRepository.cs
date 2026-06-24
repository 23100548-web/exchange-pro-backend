using UESAN.ExchangePro.CORE.Core.Entities;

public interface IRetiroRepository
{
    Task<bool> Insert(Retiros retiro);
    Task<IEnumerable<Retiros>> GetByUsuario(long idUsuario);
}
