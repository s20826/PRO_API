using Application.DTO;
using Application.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class UpdateWeterynarzCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public WeterynarzUpdateRequest request { get; set; }
    }

    public class UpdateWeterynarzCommandHandle : IRequestHandler<UpdateWeterynarzCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateWeterynarzCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var konto = context.Osobas.Where(x => x.IdOsoba == id).First();
            var weterynarz = context.Weterynarzs.Where(x => x.IdOsoba == id).First();

            konto.Imie = req.request.Imie;
            konto.Nazwisko = req.request.Nazwisko;

            weterynarz.DataUrodzenia = req.request.DataUrodzenia;
            weterynarz.Pensja = req.request.Pensja;
            weterynarz.DataZatrudnienia = req.request.DataZatrudnienia;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}