using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Weterynarz
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

            var weterynarz = context.Osobas.Where(x => x.IdOsoba == id).FirstOrDefault();
            if (weterynarz != null)
            {
                throw new NotFoundException();
            }

            weterynarz.Haslo = "";
            weterynarz.Salt = "";
            weterynarz.RefreshToken = "";
            weterynarz.Email = "";
            weterynarz.NumerTelefonu = "";

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
