using DisneyAPI.ViewModel.Mail;
using System.Threading.Tasks;

namespace DisneyAPI.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(MailServiceReqVM sendMail);
    }
}
