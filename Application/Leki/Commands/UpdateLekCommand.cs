using Application.DTO.Requests;
using Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class UpdateLekCommand : IRequest<int>
    {
        public string ID_lek { get; set; }
        public LekRequest request { get; set; }
    }

    public class UpdateLekCommandHandle : IRequestHandler<UpdateLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public UpdateLekCommandHandle(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(UpdateLekCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_lek);

            var lek = context.Leks.Where(x => x.IdLek.Equals(id)).First();
            lek.Nazwa = req.request.Nazwa;
            lek.JednostkaMiary = req.request.JednostkaMiary;

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}