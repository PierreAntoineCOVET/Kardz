using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;

namespace EventHandlers.Queries.GetShuffledCards
{
    /// <summary>
    /// Validator for <see cref="GetShuffledCardsQuery"/>.
    /// </summary>
    public class GetShuffledCardsQueryValidator : AbstractValidator<GetShuffledCardsQuery>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public GetShuffledCardsQueryValidator()
        {
            RuleFor(query => query.GameType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
