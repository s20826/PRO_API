using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramKlientQueryValidator : AbstractValidator<HarmonogramKlientQuery>
    {
        public HarmonogramKlientQueryValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}
