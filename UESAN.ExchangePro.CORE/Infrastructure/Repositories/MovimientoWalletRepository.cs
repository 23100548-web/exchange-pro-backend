using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Infrastructure.Data;

namespace UESAN.ExchangePro.Infrastructure.Repositories
{
    public class MovimientoWalletRepository : IMovimientoWalletRepository
    {
        private readonly ExchangeProDbContext _context;

        public MovimientoWalletRepository(ExchangeProDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovimientosWallet>> GetByWalletId(long idWallet, int? idMoneda = null)
        {
            var query = _context.MovimientosWallet
                .Include(m => m.IdMonedaNavigation)
                .Where(m => m.IdWallet == idWallet);

            if (idMoneda.HasValue)
                query = query.Where(m => m.IdMoneda == idMoneda.Value);

            return await query
                .OrderByDescending(m => m.FechaMovimiento)
                .ToListAsync();
        }

        public async Task<bool> Insert(MovimientosWallet movimiento)
        {
            await _context.MovimientosWallet.AddAsync(movimiento);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
