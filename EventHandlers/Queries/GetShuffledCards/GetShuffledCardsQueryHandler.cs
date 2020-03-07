using Domain.Domain.Services;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetShuffledCards
{
    /// <summary>
    /// Handler for <see cref="GetShuffledCardsQuery"/>.
    /// </summary>
    public class GetShuffledCardsQueryHandler : IRequestHandler<GetShuffledCardsQuery, IEnumerable<CardsEnum>>
    {
        /// <summary>
        /// Process GetShuffledCardsQuery.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of cards <see cref="IEnumerable{CardsEnum}"./></returns>
        public Task<IEnumerable<CardsEnum>> Handle(GetShuffledCardsQuery request, CancellationToken cancellationToken)
        {
            var deck = CardsService.GetCardDeck((GamesEnum)request.GameType);
            var suffledCards = deck.Shuffle();

            return Task.FromResult(suffledCards);
        }
    }
}
