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
        public int ID_osoba { get; set; }
    }

    public class DeleteWeterynarzCommandHandle : IRequestHandler<DeleteWeterynarzCommand, int>
    {
        private readonly IWeterynarzRepository repository;

        public DeleteWeterynarzCommandHandle(IWeterynarzRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(DeleteWeterynarzCommand req, CancellationToken cancellationToken)
        {
            return await repository.DeleteWeterynarz(req.ID_osoba);
        }
    }
}
