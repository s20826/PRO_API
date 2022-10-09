using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leki.Queries
{
    public class StanLekuQueryValidator : AbstractValidator<StanLekuQuery>
    {
        public StanLekuQueryValidator()
        {
            RuleFor(x => x.ID_stan_leku).NotEmpty();
        }
    }
}
