using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Infrastructure.Data;

public class CalificacionRepository : ICalificacionRepository
{
    private readonly ExchangeProDbContext _context;
    public CalificacionRepository(ExchangeProDbContext context) => _context = context;

    public async Task<bool> Insert(Calificaciones calificacion)
    {
        await _context.Calificaciones.AddAsync(calificacion);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsByTransaccionAndUser(long idTransaccion, long idUsuarioCalificador)
    {
        return await _context.Calificaciones
            .AnyAsync(c => c.IdTransaccion == idTransaccion && c.UsuarioCalificador == idUsuarioCalificador);
    }

    public async Task<IEnumerable<Calificaciones>> GetByUsuarioCalificado(long idUsuario)
    {
        return await _context.Calificaciones
            .Where(c => c.UsuarioCalificado == idUsuario)
            .ToListAsync();
    }
}