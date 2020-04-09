using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    public class AddPlayerToLobbyCommandValidator : AbstractValidator<AddPlayerToLobbyCommand>
    {
        public AddPlayerToLobbyCommandValidator()
        {
            RuleFor(query => query.PlayerId).NotNull();

            //RuleFor(query => query.GamesType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
