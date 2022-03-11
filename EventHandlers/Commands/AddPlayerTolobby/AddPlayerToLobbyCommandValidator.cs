using Domain.Enums;
using FluentValidation;
using System;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    /// <summary>
    /// Validator for <see cref="AddPlayerToLobbyCommand"/>.
    /// </summary>
    public class AddPlayerToLobbyCommandValidator : AbstractValidator<AddPlayerToLobbyCommand>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AddPlayerToLobbyCommandValidator()
        {
            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query.GameType).IsInEnum();
        }
    }
}
