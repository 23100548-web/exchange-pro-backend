using System.Threading.Tasks;

namespace UESAN.ExchangePro.CORE.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
