using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;
using UESAN.ExchangePro.CORE.Infrastructure.Data;

namespace UESAN.ExchangePro.Infrastructure.Repositories
{
    public class OfertaRepository : IOfertaRepository
    {
        private readonly ExchangeProDbContext _context;

        public OfertaRepository(ExchangeProDbContext context)
        {
            _context = context;
        }

        // Registrar una nueva oferta
        public async Task<bool> Insert(Ofertas oferta)
        {
            await _context.Ofertas.AddAsync(oferta);
            return await _context.SaveChangesAsync() > 0;
        }

        // Listar todas las ofertas activas para el mercado (con navegación al usuario)
        public async Task<IEnumerable<Ofertas>> GetAllActivas()
        {
            return await _context.Ofertas
                .Where(o => o.Estado == "ACTIVA")
                .Include(o => o.IdUsuarioNavigation)
                .Include(o => o.MonedaEntregaNavigation)
                .Include(o => o.MonedaRecibeNavigation)
                .ToListAsync();
        }

        // Consultar ofertas de un usuario específico
        public async Task<IEnumerable<Ofertas>> GetByUsuario(long idUsuario)
        {
            return await _context.Ofertas
                .Where(o => o.IdUsuario == idUsuario)
                .Include(o => o.IdUsuarioNavigation)
                .Include(o => o.MonedaEntregaNavigation)
                .Include(o => o.MonedaRecibeNavigation)
                .ToListAsync();
        }

        // Opcional: Obtener una oferta por su ID
        public async Task<Ofertas?> GetById(int idOferta)
        {
            return await _context.Ofertas
                .Include(o => o.IdUsuarioNavigation)
                .Include(o => o.MonedaEntregaNavigation)
                .Include(o => o.MonedaRecibeNavigation)
                .FirstOrDefaultAsync(o => o.IdOferta == idOferta);
        }

        // Opcional: Actualizar el estado (ej. cambiar a "CERRADA" al realizar transacción)
        public async Task<bool> Update(Ofertas oferta)
        {
            _context.Ofertas.Update(oferta);
            return await _context.SaveChangesAsync() > 0;
        }
        // Cancelar Oferta y liberar el Escrow (retención)
        public async Task<bool> CancelarOferta(long idOferta, long idUsuario)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Buscamos la oferta y validamos
                var oferta = await _context.Ofertas.FindAsync(idOferta);
                if (oferta == null) throw new Exception("Oferta no encontrada.");
                if (oferta.IdUsuario != idUsuario) throw new Exception("No tienes permiso para cancelar esta oferta.");
                if (oferta.Estado != "ACTIVA") throw new Exception("Solo se pueden cancelar ofertas que estén ACTIVAS.");

                // 2. Buscamos la Wallet del usuario
                var wallet = await _context.Wallets
                    .Include(w => w.WalletSaldos)
                    .FirstOrDefaultAsync(w => w.IdUsuario == idUsuario);

                if (wallet == null) throw new Exception("Wallet no encontrada.");

                // 3. Devolvemos el dinero del saldo retenido al disponible
                var saldoMoneda = wallet.WalletSaldos.FirstOrDefault(s => s.IdMoneda == oferta.MonedaEntrega);
                if (saldoMoneda == null || saldoMoneda.SaldoRetenido < oferta.MontoOfertado)
                    throw new Exception("Error de consistencia: No hay suficiente saldo retenido para devolver.");

                saldoMoneda.SaldoRetenido -= oferta.MontoOfertado;
                saldoMoneda.SaldoDisponible += oferta.MontoOfertado;

                // 4. Cambiamos el estado de la oferta
                oferta.Estado = "CANCELADA";

                // 5. Guardamos todo de golpe
                _context.Ofertas.Update(oferta);
                _context.Wallets.Update(wallet);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}