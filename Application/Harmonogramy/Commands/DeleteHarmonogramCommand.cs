using Application.Interfaces;
using Domain.Enums;
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
    public class DeleteHarmonogramCommand : IRequest<object>
    {
        public DateTime Data { get; set; }
    }

    public class DeleteHarmonogramCommandHandler : IRequestHandler<DeleteHarmonogramCommand, object>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteHarmonogramCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<object> Handle(DeleteHarmonogramCommand req, CancellationToken cancellationToken)
        {
            var harmonograms = context.Harmonograms.Where(x => x.DataRozpoczecia.Date.Equals(req.Data)).ToList();

            if (!harmonograms.Any())
            {
                throw new Exception("Harmonogram nie istnieje w dniu: " + req.Data);
            }

            foreach(Harmonogram h in harmonograms)
            {
                if (h.IdWizyta.HasValue)
                {
                    var x = context.Wizyta.Where(x => x.IdWizyta.Equals(h.IdWizyta)).First();
                    x.Status = WizytaStatus.AnulowanaKlinika.ToString();
                }

                context.Harmonograms.Remove(h);
            }

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}