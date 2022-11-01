using Application.Interfaces;
using Domain.Models;
using Domain.Enums;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Common.Exceptions;
using Domain;

namespace Application.Wizyty.Commands
{
    public class UmowWizyteKlientCommand : IRequest<int>
    {
        public string ID_klient { get; set; }
        public string ID_pacjent { get; set; }
        public string ID_Harmonogram { get; set; }
        public string Notatka { get; set; }
    }

    public class UmowWizyteKlientCommandHandle : IRequestHandler<UmowWizyteKlientCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IWizytaRepository wizytaRepository;
        private readonly IEmailSender sender;
        public UmowWizyteKlientCommandHandle(IKlinikaContext klinikaContext, IHash _hash, IWizytaRepository _wizytaRepository, IEmailSender emailSender)
        {
            context = klinikaContext;
            hash = _hash;
            wizytaRepository = _wizytaRepository;
            sender = emailSender;
        }

        public async Task<int> Handle(UmowWizyteKlientCommand req, CancellationToken cancellationToken)
        {
            int id1 = hash.Decode(req.ID_klient);
            int id_harmonogram = hash.Decode(req.ID_Harmonogram);

            if (!wizytaRepository.IsWizytaAbleToCreate(context.Wizyta.Where(x => x.IdOsoba == id1).ToList()))
            {
                throw new ConstraintException("To many wizytas made", GlobalValues.MAX_UMOWIONYCH_WIZYT);
            }

            var result = await context.Wizyta.AddAsync(new Wizytum
            {
                IdOsoba = id1,
                IdPacjent = req.ID_pacjent != "0" ? hash.Decode(req.ID_pacjent) : null,
                Opis = "",
                NotatkaKlient = req.Notatka,
                Status = WizytaStatus.Zaplanowana.ToString(),
                Cena = 0,
                CzyOplacona = false
            });

            var harmonogram = context.Harmonograms.Where(x => x.IdHarmonogram == id_harmonogram).FirstOrDefault();
            harmonogram.IdWizyta = result.Entity.IdWizyta;

            await context.SaveChangesAsync(cancellationToken);

            //wysłanie maila z potwierdzeniem umówienia wizyty
            var to = context.Osobas.Where(x => x.IdOsoba.Equals(id1)).First().Email;
            var weterynarz = context.Osobas.Where(x => x.IdOsoba.Equals(harmonogram.WeterynarzIdOsoba)).Select(x => x.Imie + " " + x.Nazwisko).First();
            await sender.SendUmowWizytaEmail(to, harmonogram.DataRozpoczecia, weterynarz);

            return 0;
        }
    }
}