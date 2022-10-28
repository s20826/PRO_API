using FluentValidation;

namespace Application.Harmonogramy.Queries
{
    public class HarmonogramAdminByIDQueryValidator : AbstractValidator<HarmonogramAdminByIDQuery>
    {
        public HarmonogramAdminByIDQueryValidator()
        {
            RuleFor(x => x.ID_osoba).NotEmpty();

            RuleFor(x => x.StartDate).NotEmpty();

            RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(y => y.StartDate);
        }
    }
}