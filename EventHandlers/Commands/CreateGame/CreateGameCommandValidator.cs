using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.CreateGame
{
    public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {
        public CreateGameCommandValidator()
        {
            RuleFor(query => query.GameType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
