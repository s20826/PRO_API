using Application.DTO.Requests;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Szczepionki.Commands
{
    public class CreateSzczepionkaCommand : IRequest<int>
    {
        public SzczepionkaRequest request { get; set; }
    }

    public class CreateSzczepionkaCommandHandler : IRequestHandler<CreateSzczepionkaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateSzczepionkaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateSzczepionkaCommand req, CancellationToken cancellationToken)
        {
            var lek = context.Leks.Add(new Lek
            {
                Nazwa = req.request.Nazwa,
                JednostkaMiary = "ml",
                Producent = req.request.Producent
            });

            context.Szczepionkas.Add(new Szczepionka
            {
                IdLek = lek.Entity.IdLek,
                Zastosowanie = req.request.Zastosowanie,
                CzyObowiazkowa = req.request.CzyObowiazkowa,
                OkresWaznosci = req.request.OkresWaznosci
            });
            
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}