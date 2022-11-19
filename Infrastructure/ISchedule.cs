using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISchedule
    {
        void SendPrzypomnienieEmail(string to);

        Task DeleteWizytaSystemAsync();
    }
}