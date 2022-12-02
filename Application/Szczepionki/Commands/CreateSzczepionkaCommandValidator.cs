using FluentValidation;

namespace Application.Szczepionki.Commands
{
    public class CreateSzczepionkaCommandValidator : AbstractValidator<CreateSzczepionkaCommand>
    {
        public CreateSzczepionkaCommandValidator()
        {
            RuleFor(x => x.request.Producent).MaximumLength(50);

            RuleFor(x => x.request.Zastosowanie).MinimumLength(2).MaximumLength(100);

            RuleFor(x => x.request.CzyObowiazkowa).NotEmpty();

            RuleFor(x => x.request.OkresWaznosci);
        }
    }
}