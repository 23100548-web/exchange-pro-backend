using Microsoft.EntityFrameworkCore;
using UESAN.ExchangePro.CORE.Core.Entities;
using UESAN.ExchangePro.CORE.Core.Interfaces;
using UESAN.ExchangePro.CORE.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UESAN.ExchangePro.CORE.Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ExchangeProDbContext _context;

        public FeedbackRepository(ExchangeProDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetAll()
        {
            return await _context.Set<Feedback>()
                .Include(f => f.IdUsuarioNavigation)
                .OrderByDescending(f => f.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Feedback> GetById(long id)
        {
            return await _context.Set<Feedback>()
                .Include(f => f.IdUsuarioNavigation)
                .FirstOrDefaultAsync(f => f.IdFeedback == id);
        }

        public async Task<IEnumerable<Feedback>> GetByUsuario(long idUsuario)
        {
            return await _context.Set<Feedback>()
                .Where(f => f.IdUsuario == idUsuario)
                .OrderByDescending(f => f.FechaCreacion)
                .ToListAsync();
        }

        public async Task<bool> Insert(Feedback feedback)
        {
            await _context.Set<Feedback>().AddAsync(feedback);
            int rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> Update(Feedback feedback)
        {
            _context.Set<Feedback>().Update(feedback);
            int rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}