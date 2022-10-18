using Application.Interfaces;
using Domain;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services
{
    public class WizytaService : IWizytaRepository
    {
        public bool IsWizytaAbleToCreate(List<Wizytum> wizytaList)
        {
            int result = wizytaList.Where(x => ((WizytaStatus)Enum.Parse(typeof(WizytaStatus), x.Status, true)).Equals(WizytaStatus.Zaplanowana)).Count();
            return result <= GlobalValues.MAX_UMOWIONYCH_WIZYT;
        }

        public bool IsWizytaAbleToCancel(DateTime wizytaDate)
        {
            return wizytaDate <= DateTime.Now.AddHours(-GlobalValues.GODZINY_DO_ANULOWANIA_WIZYTY_BEZ_KONSEKWENCJI);
        }

        public (DateTime, DateTime) GetWizytaDates(List<Harmonogram> harmonograms)
        {
            var result = harmonograms.OrderBy(x => x.DataRozpoczecia).ToList();
            var rozpoczecie = result.First().DataRozpoczecia;
            var zakonczenie = result.Last().DataZakonczenia;
            return (rozpoczecie, zakonczenie);
        }
    }
}
