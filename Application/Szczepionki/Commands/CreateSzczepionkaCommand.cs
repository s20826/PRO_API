using Application.DTO.Requests;
using Application.Interfaces;
using Domain;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

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
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var lek = context.Leks.Add(new Lek
                    {
                        Nazwa = req.request.Nazwa,
                        JednostkaMiary = GlobalValues.SZCZEPIONKA_JEDNOSTKA,
                        Producent = req.request.Producent
                    });

                    await context.SaveChangesAsync(cancellationToken);

                    context.Szczepionkas.Add(new Szczepionka
                    {
                        IdLek = lek.Entity.IdLek,
                        Zastosowanie = req.request.Zastosowanie,
                        CzyObowiazkowa = req.request.CzyObowiazkowa,
                        OkresWaznosci = req.request.OkresWaznosci.Value.Ticks
                    });


                    await context.SaveChangesAsync(cancellationToken);
                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw new Exception();
                }

                transaction.Dispose();
                return 0;
            }
        }
    }
}