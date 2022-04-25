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
    public class UpdateStanLekuCommand : IRequest<int>
    {
        public int ID_stan_leku { get; set; }
        public StanLekuRequest request { get; set; }
    }

    public class UpdateStanLekuCommandHandle : IRequestHandler<UpdateStanLekuCommand, int>
    {
        private readonly ILekRepository repository;

        public UpdateStanLekuCommandHandle(ILekRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(UpdateStanLekuCommand req, CancellationToken cancellationToken)
        {
            return await repository.UpdateStanLeku(req.ID_stan_leku, req.request);
        }
    }
}