using FluentValidation;

namespace Application.Konto.Commands
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.request.CurrentHaslo).NotEmpty().MinimumLength(8).MaximumLength(30);

            RuleFor(x => x.request.NewHaslo).NotEmpty().MinimumLength(8).MinimumLength(30).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");

            RuleFor(x => x.request.NewHaslo2).NotEmpty().MinimumLength(8).MinimumLength(30).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$").NotEqual(y => y.request.NewHaslo);
        }
    }
}
