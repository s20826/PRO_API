using Application.Common.Exceptions;
using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class UpdateKontoCommand : IRequest<int>
    {
        public string ID_osoba { get; set; }
        public KontoUpdateRequest request { get; set; }
    }

    public class UpdateKontoCommandHandle : IRequestHandler<UpdateKontoCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateKontoCommandHandle(IKlinikaContext klinikaContext, IPasswordRepository password, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateKontoCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_osoba);

            var user = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (user == null)
            {
                throw new UserNotAuthorizedException();
            }

            user.NumerTelefonu = req.request.NumerTelefonu;
            user.Email = req.request.Email;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}