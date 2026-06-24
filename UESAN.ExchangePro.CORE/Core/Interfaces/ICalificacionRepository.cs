using UESAN.ExchangePro.CORE.Core.Entities;

public interface ICalificacionRepository
{
    Task<bool> Insert(Calificaciones calificacion);
    Task<bool> ExistsByTransaccionAndUser(long idTransaccion, long idUsuarioCalificador);
    Task<IEnumerable<Calificaciones>> GetByUsuarioCalificado(long idUsuario);
}