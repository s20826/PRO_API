using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wizyty.Commands
{
    public class DeleteWizytaKlientCommandValidator : AbstractValidator<DeleteWizytaKlientCommand>
    {
        public DeleteWizytaKlientCommandValidator()
        {
            RuleFor(x => x.ID_wizyta).NotEmpty();
        }
    }
}
