using System.Collections.Generic;
using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.Entities;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface INotificacionesRepository
    {
        Task<List<Notificaciones>> GetForUsuario(long idUsuario, bool esAdmin);
        Task<int> GetUnreadCount(long idUsuario, bool esAdmin);
        Task<bool> MarkAsRead(long idNotificacion, long idUsuario, bool esAdmin);
        Task<bool> Create(Notificaciones notificacion);
    }
}
