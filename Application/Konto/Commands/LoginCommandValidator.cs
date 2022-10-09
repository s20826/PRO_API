using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Konto.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.request.NazwaUzytkownika).MinimumLength(2).MaximumLength(30);

            RuleFor(x => x.request.Haslo).MinimumLength(8).MaximumLength(30);
        }
    }
}
