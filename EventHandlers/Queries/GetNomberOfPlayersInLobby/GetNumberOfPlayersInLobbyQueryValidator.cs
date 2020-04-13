using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

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
