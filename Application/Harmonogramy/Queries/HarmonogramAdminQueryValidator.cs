using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramAdminQueryValidator : AbstractValidator<HarmonogramAdminQuery>
    {
        public HarmonogramAdminQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.StartDate).NotEmpty();

            RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(y => y.StartDate);
        }
    }
}
