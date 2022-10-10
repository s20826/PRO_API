using FluentValidation;

namespace Application.Klienci.Commands
{
    public class CreateKlientCommandValidator : AbstractValidator<CreateKlientCommand>
    {
        public CreateKlientCommandValidator()
        {
            RuleFor(x => x.request.Imie).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Nazwisko).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Email).NotEmpty().MinimumLength(6).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)");

            RuleFor(x => x.request.NumerTelefonu).NotEmpty().Matches(@"^(\+?[0-9]{9,11})").MaximumLength(12);

            RuleFor(x => x.request.NazwaUzytkownika).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Haslo).NotEmpty().MinimumLength(8).MaximumLength(30).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");

            RuleFor(x => x.request.Haslo2).NotEmpty().MinimumLength(8).MaximumLength(30).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
        }
    }
}
