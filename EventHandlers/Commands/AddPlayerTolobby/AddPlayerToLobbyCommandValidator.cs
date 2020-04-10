using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;

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
            RuleFor(query => query.PlayerId).NotNull();

            //RuleFor(query => query.GamesType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
