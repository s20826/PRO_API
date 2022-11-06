using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class DeleteWeterynarzCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
    }

    public class DeleteWeterynarzCommandHandle : IRequestHandler<DeleteWeterynarzCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteWeterynarzCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteWeterynarzCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var weterynarz = context.Osobas.Where(x => x.IdOsoba == id).First();
            weterynarz.Nazwisko = weterynarz.Nazwisko.ElementAt(0).ToString();
            weterynarz.Haslo = "";
            weterynarz.Salt = "";
            weterynarz.RefreshToken = "";
            weterynarz.Email = "";
            weterynarz.NumerTelefonu = "";

            var x = context.GodzinyPracies.Where(x => x.IdOsoba == id).ToList();
            if (x.Any())
            {
                context.GodzinyPracies.RemoveRange(x);
            }

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}