using Application.Interfaces;
using Domain.Models;
using Domain.Enums;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public UmowWizyteKlientCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UmowWizyteKlientCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_klient, req.ID_pacjent);
            int id_harmonogram = hash.Decode(req.ID_Harmonogram);

            var result = await context.Wizyta.AddAsync(new Wizytum
            {
                IdOsoba = id1,
                IdPacjent = id2,
                Opis = "",
                NotatkaKlient = req.Notatka,
                Status = WizytaStatus.Zaplanowana.ToString(),
                Cena = 0,
                CzyOplacona = false
            });

            await context.SaveChangesAsync(cancellationToken);

            var harmonogram = context.Harmonograms.Where(x => x.IdHarmonogram == id_harmonogram).FirstOrDefault();
            harmonogram.IdWizyta = result.Entity.IdWizyta;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}