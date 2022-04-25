using Application.DTO.Request;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Lek
{
    public class CreateStanLekuCommand : IRequest<int>
    {
        public int ID_lek { get; set; }
        public StanLekuRequest request {get; set;}
    }

    public class CreateStanLekuCommandHandle : IRequestHandler<CreateStanLekuCommand, int>
    {
        private readonly ILekRepository repository;

        public CreateStanLekuCommandHandle(ILekRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(CreateStanLekuCommand req, CancellationToken cancellationToken)
        {
            return await repository.AddStanLeku(req.ID_lek, req.request);
        }
    }
}
