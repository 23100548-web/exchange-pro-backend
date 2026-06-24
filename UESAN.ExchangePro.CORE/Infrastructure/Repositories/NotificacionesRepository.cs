using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;
using UESAN.ExchangePro.CORE.Infrastructure.Data;

namespace UESAN.ExchangePro.CORE.Infrastructure.Repositories
{
    public class NotificacionesRepository : INotificacionesRepository
    {
        private readonly ExchangeProDbContext _context;

        public NotificacionesRepository(ExchangeProDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notificaciones>> GetForUsuario(long idUsuario, bool esAdmin)
        {
            if (esAdmin)
            {
                return await _context.Notificaciones
                    .Where(n => n.IdUsuario == null)
                    .OrderByDescending(n => n.Fecha)
                    .ToListAsync();
            }

            return await _context.Notificaciones
                .Where(n => n.IdUsuario == idUsuario)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCount(long idUsuario, bool esAdmin)
        {
            if (esAdmin)
            {
                return await _context.Notificaciones
                    .Where(n => n.IdUsuario == null && n.Leido == false)
                    .CountAsync();
            }

            return await _context.Notificaciones
                .Where(n => n.IdUsuario == idUsuario && n.Leido == false)
                .CountAsync();
        }

        public async Task<bool> MarkAsRead(long idNotificacion, long idUsuario, bool esAdmin)
        {
            var notificacion = await _context.Notificaciones
                .FirstOrDefaultAsync(n => n.IdNotificacion == idNotificacion &&
                    ((esAdmin && n.IdUsuario == null) || (!esAdmin && n.IdUsuario == idUsuario)));

            if (notificacion == null)
                return false;

            notificacion.Leido = true;
            _context.Notificaciones.Update(notificacion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Create(Notificaciones notificacion)
        {
            await _context.Notificaciones.AddAsync(notificacion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
