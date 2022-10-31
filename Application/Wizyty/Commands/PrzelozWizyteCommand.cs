﻿using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Wizyty.Commands
{
    public class PrzelozWizyteCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }   //stare
        public string ID_klient { get; set; }
        public string ID_pacjent { get; set; }
        public string ID_Harmonogram { get; set; }  //nowe
        public string Notatka { get; set; }
    }

    public class PrzelozWizyteCommandHandle : IRequestHandler<PrzelozWizyteCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        public PrzelozWizyteCommandHandle(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
        }

        public async Task<int> Handle(PrzelozWizyteCommand req, CancellationToken cancellationToken)
        {
            (int klientID, int wizytaID) = hash.Decode(req.ID_klient, req.ID_wizyta);
            int harmonogramID = hash.Decode(req.ID_Harmonogram);

            var wizyta = context.Wizyta.Where(x => x.IdWizyta.Equals(wizytaID)).FirstOrDefault();
            var oldHarmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(wizytaID)).ToList();

            if (!((WizytaStatus)Enum.Parse(typeof(WizytaStatus), wizyta.Status, true)).Equals(WizytaStatus.Zaplanowana))
            {
                throw new Exception();
            }

            if (!klientID.Equals(wizyta.IdOsoba))
            {
                throw new UserNotAuthorizedException();
            }

            if (!oldHarmonograms.Any())
            {
                throw new NotFoundException();
            }

            (DateTime rozpoczecie, DateTime zakonczenie) = wizytaRepository.GetWizytaDates(oldHarmonograms);
            if (!wizytaRepository.IsWizytaAbleToCancel(rozpoczecie))
            {
                //naliczenie kary lub wysłanie powiadomienia
            }

            //wyliczenie długości przekładanej wizyty
            int wizytaLength = oldHarmonograms.Count();

            //ID weterynarza z nowej wizyty
            int weterynarzID = context.Harmonograms.Where(x => x.IdHarmonogram.Equals(harmonogramID)).First().WeterynarzIdOsoba;
            var newDataRozpoczecia = context.Harmonograms.Where(x => x.IdHarmonogram.Equals(harmonogramID)).First().DataRozpoczecia;

            //usuwanie poprzednich zarezerwowanych terminów z harmonogramu
            foreach (Harmonogram h in oldHarmonograms)
            {
                h.IdWizyta = null;
            }

            //nowe harmonogramy wyciągnięte na podstawie nowego ID harmonogramu
            var newHarmonograms = context.Harmonograms
                .Where(x => x.WeterynarzIdOsoba.Equals(weterynarzID) && x.DataRozpoczecia >= newDataRozpoczecia)
                .OrderBy(x => x.DataRozpoczecia)
                .Take(wizytaLength)
                .ToList();

            //sprawdzenie czy są dostępne terminy do danego weterynarz w harmonogramie
            if (!wizytaRepository.IsWizytaAbleToReschedule(newHarmonograms, newDataRozpoczecia))
            {
                throw new NotFoundException();
            }

            for (int i = 0; i <= newHarmonograms.Count; i++)
            {
                newHarmonograms.ElementAt(i).IdWizyta = wizytaID;
            }

            wizyta.IdPacjent = req.ID_pacjent != null ? hash.Decode(req.ID_pacjent) : null;
            wizyta.NotatkaKlient = req.Notatka;

            int result = await context.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}