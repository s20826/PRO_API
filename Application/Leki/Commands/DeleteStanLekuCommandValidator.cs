﻿using Application.Commands.Lek;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Leki.Commands
{
    public class DeleteStanLekuCommandValidator : AbstractValidator<DeleteStanLekuCommand>
    {
        public DeleteStanLekuCommandValidator()
        {
            RuleFor(x => x.ID_stan_leku).NotEmpty();
        }
    }
}