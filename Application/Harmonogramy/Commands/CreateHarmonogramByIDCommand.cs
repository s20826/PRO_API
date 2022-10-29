using Application.Interfaces;
using Domain;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Commands
{
    public class CreateHarmonogramByIDCommand : IRequest<int>
    {
        public string ID_weterynarz { get; set; }
        public DateTime Data { get; set; }
    }

    public class CreateHarmonogramByIDCommandHandler : IRequestHandler<CreateHarmonogramByIDCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogram;
        public CreateHarmonogramByIDCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogram = harmonogramRepository;
        }

        public async Task<int> Handle(CreateHarmonogramByIDCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_weterynarz);

            if (context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.Data) && x.WeterynarzIdOsoba.Equals(id)).Any())
            {
                throw new Exception("Harmonogram już istnieje");
            }

            int dzienRequest = (int)req.Data.DayOfWeek;
            var godzinyPracy = context.GodzinyPracies.Where(x => x.DzienTygodnia == dzienRequest && x.IdOsoba.Equals(id)).First();
            var count = harmonogram.HarmonogramCount(godzinyPracy);

            for (int i = 0; i < count; i++)
            {
                var s = godzinyPracy.GodzinaRozpoczecia;
                context.Harmonograms.Add(new Harmonogram
                {
                    IdWizyta = null,
                    WeterynarzIdOsoba = id,
                    DataRozpoczecia = DateTime.Today + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY)),
                    DataZakonczenia = DateTime.Today + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY) + GlobalValues.DLUGOSC_WIZYTY)
                });
            }

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}