using FluentValidation;
using System;

namespace Application.Urlopy.Commands
{
    public class CreateUrlopCommandValidator : AbstractValidator<CreateUrlopCommand>
    {
        public CreateUrlopCommandValidator()
        {
            RuleFor(x => x.request.ID_weterynarz).NotEmpty();

            RuleFor(x => x.request.Data).GreaterThanOrEqualTo(DateTime.Now.Date);
        }
    }
}