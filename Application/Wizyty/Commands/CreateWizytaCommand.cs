using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class CreateWizytaCommand : IRequest<int>
    {
        public string ID_klient { get; set; }
        public string ID_pacjent { get; set; }
        public string ID_Harmonogram { get; set; }
        public string Notatka { get; set; }
    }

    public class HarmonogramAdminQueryHandle : IRequestHandler<CreateWizytaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public HarmonogramAdminQueryHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateWizytaCommand req, CancellationToken cancellationToken)
        {
            (int id1, int id2) = hash.Decode(req.ID_klient, req.ID_pacjent);
            int id_harmonogram = hash.Decode(req.ID_Harmonogram);

            var harmonogram = context.Harmonograms.Where(x => x.IdHarmonogram == id_harmonogram).FirstOrDefault();
            harmonogram.KlientIdOsoba = id1;
            harmonogram.IdPacjent = id2;

            context.Wizyta.Add(new Wizytum
            {
                IdHarmonogram = id_harmonogram,
                Opis = "",
                NotatkaKlient = req.Notatka,
                Status = WizytaStatus.Zaplanowana.ToString(),
                Cena = 0,
                CzyOplacona = false
            });

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
