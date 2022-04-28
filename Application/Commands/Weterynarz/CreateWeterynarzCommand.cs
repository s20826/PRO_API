using Application.DTO;
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
    public class CreateWeterynarzCommand : IRequest<int>
    {
        public WeterynarzCreateRequest request { get; set; }
    }

    public class CreateWeterynarzCommandHandle : IRequestHandler<CreateWeterynarzCommand, int>
    {
        private readonly IWeterynarzRepository repository;

        public CreateWeterynarzCommandHandle(IWeterynarzRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(CreateWeterynarzCommand req, CancellationToken cancellationToken)
        {
            return await repository.AddWeterynarz(req.request);
        }
    }
}