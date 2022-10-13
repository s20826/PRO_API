using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specjalizacje.Commands
{
    public class CreateSpecjalizacjaCommandValidator : AbstractValidator<CreateSpecjalizacjaCommand>
    {
        public CreateSpecjalizacjaCommandValidator()
        {
            RuleFor(x => x.request.Nazwa).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Opis).MaximumLength(300);
        }
    }
}
