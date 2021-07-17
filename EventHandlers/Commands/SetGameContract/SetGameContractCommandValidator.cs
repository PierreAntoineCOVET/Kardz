using Domain.Enums;
using FluentValidation;
using FluentValidation.Validators;
using System;
using EnumValidator = EventHandlers.Validator.EnumValidator;

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

            RuleFor(query => query).Custom(ContractIsValid);
        }

        private void ContractIsValid(SetGameContractCommand query, CustomContext context)
        {
            if ((query.Color.HasValue && !query.Value.HasValue)
                || (!query.Color.HasValue && query.Value.HasValue))
            {
                context.AddFailure($"Invalid contract {{ 'color':'{query.Color}', 'value':'{query.Value}'}}.");
            }
        }
    }
}
