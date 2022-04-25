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
    public class DeleteStanLekuCommand : IRequest<int>
    {
        public int ID_stan_leku { get; set; }
    }

    public class DeleteStanLekuCommandHandle : IRequestHandler<DeleteStanLekuCommand, int>
    {
        private readonly ILekRepository repository;

        public DeleteStanLekuCommandHandle(ILekRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<int> Handle(DeleteStanLekuCommand req, CancellationToken cancellationToken)
        {
            return await repository.DeleteStanLeku(req.ID_stan_leku);
        }
    }
}