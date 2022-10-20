using Application.Common.Exceptions;
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
        public string ID_wizyta { get; set; }
        public string ID_klient { get; set; }
        public string ID_pacjent { get; set; }
        public string ID_Harmonogram { get; set; }
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
            var harmonograms = context.Harmonograms.Where(x => x.IdWizyta.Equals(wizytaID)).ToList();

            if (!((WizytaStatus)Enum.Parse(typeof(WizytaStatus), wizyta.Status, true)).Equals(WizytaStatus.Zaplanowana))
            {
                throw new Exception();
            }

            if (!klientID.Equals(wizyta.IdOsoba))
            {
                throw new UserNotAuthorizedException();
            }

            if (!harmonograms.Any())
            {
                throw new NotFoundException();
            }

            (DateTime rozpoczecie, DateTime zakonczenie) = wizytaRepository.GetWizytaDates(harmonograms);

            if (!wizytaRepository.IsWizytaAbleToCancel(rozpoczecie))
            {
                //naliczenie kary lub wysłanie powiadomienia
            }

            //wyliczenie długości przekładanej wizyty
            int wizytaLength = harmonograms.Count();

            //ID weterynarza z nowej wizyty
            int weterynarzID = context.Harmonograms.Where(x => x.IdWizyta.Equals(wizytaID)).First().WeterynarzIdOsoba;
            var newDataRozpoczecia = context.Harmonograms.Where(x => x.IdHarmonogram.Equals(harmonogramID)).First().DataRozpoczecia;

            int result = 0;
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var newHarmonograms = context.Harmonograms
                        .Where(x => x.WeterynarzIdOsoba.Equals(weterynarzID) && x.DataRozpoczecia >= newDataRozpoczecia)
                        .Take(wizytaLength)
                        .ToList();

                    //sprawdzenie czy są dostępne terminy do danego weterynarz w harmonogramie
                    if (!wizytaRepository.IsWizytaAbleToReschedule(newHarmonograms, newDataRozpoczecia))
                    {
                        throw new NotFoundException();
                    }



                    for (int i = 0; i <= harmonograms.Count; i++)
                    {
                        harmonograms.ElementAt(i).IdWizyta = wizytaID;
                    }

                    wizyta.IdPacjent = req.ID_pacjent != "0" ? hash.Decode(req.ID_pacjent) : null;
                    wizyta.NotatkaKlient = req.Notatka;

                    //usuwanie poprzednich zarezerwowanych terminów z harmonogramu
                    foreach (Harmonogram h in harmonograms)
                    {
                        h.IdWizyta = null;
                    }

                    result = await context.SaveChangesAsync(cancellationToken);
                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw new Exception();
                }

                transaction.Dispose();
            }
            
            return result;
        }
    }
}