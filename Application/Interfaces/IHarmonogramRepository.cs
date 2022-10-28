using Domain.Models;

namespace Application.Interfaces
{
    public interface IHarmonogramRepository
    {
        public int HarmonogramCount(GodzinyPracy godziny);
    }
}