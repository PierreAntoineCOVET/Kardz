using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;
using System;

namespace EventHandlers.Commands.SetGameContract
{
    public class SetGameContractCommandValidator : AbstractValidator<SetGameContractCommand>
    {
        public SetGameContractCommandValidator()
        {
            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query.GameId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query.Color).Custom(EnumValidator.Validate<ColorEnum>);

            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));
        }
    }
}
