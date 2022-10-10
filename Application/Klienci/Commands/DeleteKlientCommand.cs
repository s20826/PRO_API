using Application.Common.Exceptions;
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
        public DeleteKlientCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteKlientCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var user = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (user != null)
            {
                throw new NotFoundException();
            }

            user.Haslo = "";
            user.Salt = "";
            user.RefreshToken = "";
            user.Email = "";
            user.NumerTelefonu = "";

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}