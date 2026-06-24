using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;
using UESAN.ExchangePro.CORE.Infrastructure.Data;

namespace UESAN.ExchangePro.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ExchangeProDbContext _context;

        public WalletRepository(ExchangeProDbContext context)
        {
            _context = context;
        }

        // Obtener la Wallet de un usuario (INCLUYENDO SUS SALDOS)
        public async Task<Wallets?> GetByUsuarioId(long idUsuario)
        {
            return await _context.Wallets
                .Include(w => w.WalletSaldos)
                .Include(w => w.MovimientosWallet)
                .FirstOrDefaultAsync(w => w.IdUsuario == idUsuario);
        }

        // Actualizar la Wallet (y sus saldos modificados)
        public async Task<bool> Update(Wallets wallet)
        {
            return await _context.SaveChangesAsync() > 0;
        }

        // Crear una nueva Wallet (Útil para cuando el usuario recién se registra)
        public async Task<bool> Insert(Wallets wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}