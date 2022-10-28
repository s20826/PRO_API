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
    public class CreateHarmonogramDefaultCommand : IRequest<object>
    {
        public DateTime Data { get; set; }
    }

    public class CreateHarmonogramDefaultCommandHandler : IRequestHandler<CreateHarmonogramDefaultCommand, object>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly IHarmonogramRepository harmonogram;
        public CreateHarmonogramDefaultCommandHandler(IKlinikaContext klinikaContext, IHash _hash, IHarmonogramRepository harmonogramRepository)
        {
            context = klinikaContext;
            hash = _hash;
            harmonogram = harmonogramRepository;
        }

        public async Task<object> Handle(CreateHarmonogramDefaultCommand req, CancellationToken cancellationToken)
        {
            if(context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.Data)).Any())
            {
                throw new Exception("Harmonogram już istnieje");
            }
            
            int dzienRequest = (int)req.Data.DayOfWeek;
            var godzinyPracy = context.GodzinyPracies.Where(x => x.DzienTygodnia == dzienRequest).ToList();
            var count = 0;

            foreach (GodzinyPracy g in godzinyPracy)
            {
                count = harmonogram.HarmonogramCount(g);
                for (int i = 0; i < count; i++)
                {
                    var s = g.GodzinaRozpoczecia;
                    context.Harmonograms.Add(new Harmonogram
                    {
                        IdWizyta = null,
                        WeterynarzIdOsoba = g.IdOsoba,
                        DataRozpoczecia = DateTime.Today + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY)),
                        DataZakonczenia = DateTime.Today + TimeSpan.FromMinutes((double)s.TotalMinutes + (i * GlobalValues.DLUGOSC_WIZYTY) + GlobalValues.DLUGOSC_WIZYTY)
                    });
                }
            }

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}