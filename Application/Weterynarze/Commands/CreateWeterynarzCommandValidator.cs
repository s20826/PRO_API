﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weterynarze.Commands
{
    public class CreateWeterynarzCommandValidator : AbstractValidator<CreateWeterynarzCommand>
    {
        public CreateWeterynarzCommandValidator()
        {
            RuleFor(x => x.request.Imie).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Nazwisko).NotEmpty().MinimumLength(2).MaximumLength(50);

            RuleFor(x => x.request.Email).NotEmpty().MinimumLength(6).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)");

            RuleFor(x => x.request.NumerTelefonu).NotEmpty().Matches(@"^(\+?[0-9]{9,11})").MaximumLength(12);

            RuleFor(x => x.request.DataUrodzenia).NotEmpty();

            RuleFor(x => x.request.DataZatrudnienia).NotEmpty();

            RuleFor(x => x.request.Pensja).NotEmpty().GreaterThan(0).LessThanOrEqualTo(99999);

            RuleFor(x => x.request.Haslo).NotEmpty().MinimumLength(8).MaximumLength(30).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$");

            RuleFor(x => x.request.Login).NotEmpty().MinimumLength(8).MaximumLength(50);
        }
    }
}