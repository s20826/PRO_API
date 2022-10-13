using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Commands
{
    public class DeleteSpecjalizacjaCommand : IRequest<int>
    {
        public string ID_specjalizacja { get; set; }
    }

    public class DeleteSpecjalizacjaCommandHandle : IRequestHandler<DeleteSpecjalizacjaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public DeleteSpecjalizacjaCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(DeleteSpecjalizacjaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_specjalizacja);

            var specjalizacja = context.Specjalizacjas.Where(x => x.IdSpecjalizacja == id).First();
            context.Specjalizacjas.Remove(specjalizacja);

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}