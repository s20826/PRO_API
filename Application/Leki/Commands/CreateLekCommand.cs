using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class CreateLekCommand : IRequest<int>
    {
        public LekRequest request { get; set; }
    }

    public class CreateLekCommandHandler : IRequestHandler<CreateLekCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateLekCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateLekCommand req, CancellationToken cancellationToken)
        {
            var result = context.Leks.Add(new Lek
            {
                Nazwa = req.request.Nazwa,
                JednostkaMiary = req.request.JednostkaMiary,
                Producent = req.request.Producent
            });

            List<string> list = new List<string>(req.request.Choroby);
            if(list.Count > 0)
            {
                int lekID = result.Entity.IdLek != null ? result.Entity.IdLek : 0;
                foreach (string a in list)
                {
                    context.ChorobaLeks.Add(new ChorobaLek
                    {
                        IdChoroba = hash.Decode(a),
                        IdLek = lekID
                    });
                }
            }
            
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}