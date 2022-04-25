using Application.DTO.Responses;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Lek
{
    public class GetStanLekuQuery : IRequest<GetStanLekuResponse>
    {
        public int ID_stan_leku { get; set; }
    }

    public class GetStanLekuQueryHandle : IRequestHandler<GetStanLekuQuery, GetStanLekuResponse>
    {
        private readonly ILekRepository repository;

        public GetStanLekuQueryHandle(ILekRepository lekRepository)
        {
            repository = lekRepository;
        }

        public async Task<GetStanLekuResponse> Handle(GetStanLekuQuery req, CancellationToken cancellationToken)
        {
            return await repository.GetLekWMagazynieById(req.ID_stan_leku);
        }
    }
}

