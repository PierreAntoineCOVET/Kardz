using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.ShuffleCards
{
    /// <summary>
    /// Validator for <see cref="ShuffleCardsCommand"/>.
    /// </summary>
    public class ShuffleCardsCommandValidator : AbstractValidator<ShuffleCardsCommand>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ShuffleCardsCommandValidator()
        {
            RuleFor(query => query.GameId).NotNull();

            RuleFor(query => query.PlayerId).NotNull();
        }
    }
}
