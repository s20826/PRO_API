using Application.Common.Exceptions;
using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Klienci.Commands
{
    public class DeleteKlientCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
    }

    public class DeleteKlientCommandHandle : IRequestHandler<DeleteKlientCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        private readonly ICache<GetKlientListResponse> cache;
        public DeleteKlientCommandHandle(IKlinikaContext klinikaContext, IHash _hash, ICache<GetKlientListResponse> _cache)
        {
            context = klinikaContext;
            hash = _hash;
            cache = _cache;
        }

        public async Task<int> Handle(DeleteKlientCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var osoba = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            osoba.Nazwisko = osoba.Nazwisko.ElementAt(0).ToString();
            osoba.Haslo = "";
            osoba.Salt = "";
            osoba.RefreshToken = "";
            osoba.Email = "";
            osoba.NumerTelefonu = "";

            int result = await context.SaveChangesAsync(cancellationToken);
            cache.Remove();

            return result;
        }
    }
}