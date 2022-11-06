using Application.Interfaces;
using Domain;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HarmonogramService : IHarmonogramRepository
    {
        public int HarmonogramCount(GodzinyPracy godziny)
        {
            var result = godziny.GodzinaZakonczenia.Subtract(godziny.GodzinaRozpoczecia);

            if(result.TotalMinutes % 30 != 0)
            {
                throw new Exception();
            }

            return (int)(result.TotalMinutes/30);
        }

        public void CreateWeterynarzHarmonograms(IKlinikaContext context, DateTime date, int id)
        {
            int dzienRequest = (int)date.DayOfWeek;
            if(context.Urlops.Where(x => x.Dzien.Date.Equals(date.Date) && x.IdOsoba.Equals(id)).Any())
            {
                return;
            }
            var godzinyPracy = context.GodzinyPracies.Where(x => x.DzienTygodnia == dzienRequest && x.IdOsoba.Equals(id)).First();
            var count = HarmonogramCount(godzinyPracy);

            for (int i = 0; i < count; i++)
            {
                var s = godzinyPracy.GodzinaRozpoczecia;
                context.Harmonograms.Add(new Harmonogram
                {
                    IdWizyta = null,
                    WeterynarzIdOsoba = id,
                    DataRozpoczecia = date.Date + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY)),
                    DataZakonczenia = date.Date + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY) + GlobalValues.DLUGOSC_WIZYTY)
                });
            }
        }

        public void DeleteHarmonograms(List<Harmonogram> harmonograms, IKlinikaContext context)
        {
            foreach (Harmonogram h in harmonograms)
            {
                if (h.IdWizyta.HasValue)
                {
                    var x = context.Wizyta.Where(x => x.IdWizyta.Equals(h.IdWizyta)).First();
                    x.Status = WizytaStatus.AnulowanaKlinika.ToString();
                }

                context.Harmonograms.Remove(h);
            }
        }
    }
}