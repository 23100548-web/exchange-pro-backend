using UESAN.ExchangePro.CORE.Core.Entities;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface IOfertaRepository
    {
        Task<bool> Insert(Ofertas oferta);
        Task<IEnumerable<Ofertas>> GetAllActivas();
        Task<IEnumerable<Ofertas>> GetByUsuario(long idUsuario);

        // AGREGA ESTA LÍNEA:
        Task<Ofertas?> GetById(int idOferta);

        Task<bool> Update(Ofertas oferta);
        Task<bool> CancelarOferta(long idOferta, long idUsuario);
    }
}