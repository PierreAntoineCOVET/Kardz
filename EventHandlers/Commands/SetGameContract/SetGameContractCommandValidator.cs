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

            RuleFor(query => query.Color).IsInEnum();

            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query).Custom(ContractIsValid);
        }

        private void ContractIsValid(SetGameContractCommand query, ValidationContext<SetGameContractCommand> context)
        {
            if ((query.Color.HasValue && !query.Value.HasValue)
                || (!query.Color.HasValue && query.Value.HasValue))
            {
                context.AddFailure($"Invalid contract {{ 'color':'{query.Color}', 'value':'{query.Value}'}}.");
            }
        }
    }
}
