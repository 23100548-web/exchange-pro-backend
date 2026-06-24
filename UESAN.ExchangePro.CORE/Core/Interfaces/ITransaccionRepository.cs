using UESAN.ExchangePro.CORE.Core.Entities;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface ITransaccionRepository
    {
        Task<bool> CrearTransaccion(Transacciones transaccion, Ofertas oferta);
        Task<Transacciones?> GetById(long idTransaccion);
        Task<IEnumerable<Transacciones>> GetByUsuario(long idUsuario);
        Task<bool> ActualizarEstado(long idTransaccion, string nuevoEstado);
        Task<bool> LiberarFondos(long idTransaccion, long idVendedor);
        Task<bool> CancelarTransaccion(long idTransaccion, long idUsuario);
        Task<Transacciones?> GetTransaccionConMetodoPago(long idTransaccion);
        Task<DatosPagoUsuario?> GetDatosPagoVendedor(long idVendedor);
        Task<bool> MarcarComoPagado(long idTransaccion, long idComprador, string rutaComprobante);

        Task<bool> PagarConWallet(long idTransaccion, long idComprador);
    }
}