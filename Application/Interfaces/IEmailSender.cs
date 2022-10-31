using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendHasloEmail(string to, string content);

        Task SendUmowWizytaEmail(string to, string data);
    }
}