using FluentValidation;

namespace Application.Choroby.Commands
{
    public class CreateChorobaCommandValidator : AbstractValidator<CreateChorobaCommand>
    {
        public CreateChorobaCommandValidator()
        {
            RuleFor(x => x.request.Nazwa).MinimumLength(2).MaximumLength(50);
        }
    }
}