using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.Entities;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuarios> GetByCorreo(string correo);
        Task<bool> CorreoExiste(string correo);
        Task<bool> Insert(Usuarios usuario);
        Task<Usuarios?> GetById(long idUsuario);
        Task<bool> Update(Usuarios usuario);
        Task<Usuarios?> GetByResetToken(string resetToken);
    }
}