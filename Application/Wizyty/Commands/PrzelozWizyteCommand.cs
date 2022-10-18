using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            (DateTime rozpoczecie, DateTime zakonczenie) = wizytaRepository.GetWizytaDates(harmonograms);

            if (!wizytaRepository.IsWizytaAbleToCancel(rozpoczecie))
            {
                //naliczenie kary lub wysłanie powiadomienia
            }

            /*var result = await context.Wizyta.AddAsync(new Wizytum
            {
                IdOsoba = id1,
                IdPacjent = req.ID_pacjent != "0" ? hash.Decode(req.ID_pacjent) : null,
                Opis = "",
                NotatkaKlient = req.Notatka,
                Status = WizytaStatus.Zaplanowana.ToString(),
                Cena = 0,
                CzyOplacona = false
            });*/

            //await context.SaveChangesAsync(cancellationToken);

            //var harmonogram = context.Harmonograms.Where(x => x.IdHarmonogram == id_harmonogram).FirstOrDefault();
            //harmonogram.IdWizyta = result.Entity.IdWizyta;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}