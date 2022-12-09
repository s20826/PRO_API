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
using System.Transactions;

namespace Application.Recepty.Commands
{
    public class CreateReceptaCommand : IRequest<int>
    {
        public string ID_wizyta { get; set; }
        public string Zalecenia { get; set; }
        public List<ReceptaLekRequest2> Leki { get; set; }
    }

    public class CreateReceptaCommandHandler : IRequestHandler<CreateReceptaCommand, int>
    {
        private readonly IKlinikaContext context;
        private readonly IHash hash;
        public CreateReceptaCommandHandler(IKlinikaContext klinikaContext, IHash _hash)
        {
            context = klinikaContext;
            hash = _hash;
        }

        public async Task<int> Handle(CreateReceptaCommand req, CancellationToken cancellationToken)
        {
            int id = hash.Decode(req.ID_wizyta);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    context.Recepta.Add(new Receptum
                    {
                        IdWizyta = id,
                        Zalecenia = req.Zalecenia
                    });

                    await context.SaveChangesAsync(cancellationToken);

                    foreach (var i in req.Leki)
                    {
                        context.ReceptaLeks.Add(new ReceptaLek
                        {
                            IdWizyta = id,
                            IdLek = hash.Decode(i.ID_Lek),
                            Ilosc = i.Ilosc,
                        });
                    }

                    await context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw new Exception(e.Message);
                }

                transaction.Dispose();
                return 0;
            }
        }
    }
}