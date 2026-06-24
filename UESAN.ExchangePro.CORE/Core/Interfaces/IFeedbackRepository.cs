using UESAN.ExchangePro.CORE.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAll();
        Task<Feedback> GetById(long id);
        Task<IEnumerable<Feedback>> GetByUsuario(long idUsuario);
        Task<bool> Insert(Feedback feedback);
        Task<bool> Update(Feedback feedback);
    }
}