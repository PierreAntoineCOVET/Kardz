using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;

namespace EventHandlers.Queries.GetNomberOfPlayersInLobby
{
    public class GetNumberOfPlayersInLobbyQueryValidator : AbstractValidator<GetNumberOfPlayersInLobbyQuery>
    {
        public GetNumberOfPlayersInLobbyQueryValidator()
        {
            RuleFor(query => query.GameType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
