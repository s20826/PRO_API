using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISchedule
    {
        void SendPrzypomnienieEmail();

        Task DeleteWizytaSystemAsync();

        void SendSzczepienieEmail();
    }
}