using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class DeleteLekCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
    }

    public class DeleteLekCommandHandler : IRequestHandler<DeleteLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteLekCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_lek);

            context.LekWMagazynies.RemoveRange(context.LekWMagazynies.Where(x => x.IdLek.Equals(id)).ToList());
            context.Leks.Remove(context.Leks.Where(x => x.IdLek.Equals(id)).First());

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}