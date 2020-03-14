using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.ShuffleCards
{
    public class ShuffleCardsCommandValidator : AbstractValidator<ShuffleCardsCommand>
    {
        public ShuffleCardsCommandValidator()
        {
            RuleFor(query => query.GameId).NotNull();

            RuleFor(query => query.PlayerId).NotNull();
        }
    }
}
