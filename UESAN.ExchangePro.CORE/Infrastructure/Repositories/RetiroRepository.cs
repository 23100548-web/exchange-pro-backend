using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Infrastructure.Data;

public class RetiroRepository : IRetiroRepository
{
    private readonly ExchangeProDbContext _context;
    public RetiroRepository(ExchangeProDbContext context) => _context = context;

    public async Task<bool> Insert(Retiros retiro)
    {
        await _context.Retiros.AddAsync(retiro);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Retiros>> GetByUsuario(long idUsuario)
    {
        return await _context.Retiros
            .Where(r => r.IdUsuario == idUsuario)
            .Include(r => r.IdMonedaNavigation)
            .OrderByDescending(r => r.FechaRetiro)
            .ToListAsync();
    }
}
