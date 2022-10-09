using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class UpdateKontoCommandValidator : AbstractValidator<UpdateKontoCommand>
    {
        public UpdateKontoCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.request.DataUrodzenia).NotEmpty().LessThan(DateTime.Now);

            RuleFor(x => x.request.Email).NotEmpty().Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            RuleFor(x => x.request.NumerTelefonu).NotEmpty().Matches(@"^(\+?[0-9]{9,11})").MaximumLength(12);

            RuleFor(x => x.request.currentHaslo).NotEmpty().MinimumLength(8).MaximumLength(30);

            RuleFor(x => x.request.newHaslo).NotEmpty().MinimumLength(8).MinimumLength(30).NotEqual(y => y.request.currentHaslo);
        }
    }
}
