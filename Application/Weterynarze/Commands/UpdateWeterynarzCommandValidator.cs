using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class UpdateWeterynarzCommandValidator : AbstractValidator<UpdateWeterynarzCommand>
    {
        public UpdateWeterynarzCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.request.Imie).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Nazwisko).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.DataUrodzenia).NotEmpty();

            RuleFor(x => x.request.DataZatrudnienia).NotEmpty();

            RuleFor(x => x.request.Pensja).NotEmpty().GreaterThan(0).LessThanOrEqualTo(99999);
        }
    }
}
