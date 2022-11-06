using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IHarmonogramRepository
    {
        int HarmonogramCount(GodzinyPracy godziny);

        void DeleteHarmonograms(List<Harmonogram> harmonograms, IKlinikaContext context);

        void CreateWeterynarzHarmonograms(IKlinikaContext context, DateTime date, int id);
    }
}