using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    public class AddPlayerToLobbyCommandValidator : AbstractValidator<AddPlayerToLobbyCommand>
    {
        public AddPlayerToLobbyCommandValidator()
        {
            RuleFor(query => query.Guid).NotNull();
        }
    }
}
