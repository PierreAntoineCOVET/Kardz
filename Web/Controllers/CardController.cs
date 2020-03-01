using Domain.Enums;
using EventHandlers.Queries.GetShuffledCards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// Controller for the cards actions.
    /// </summary>
    public class CardController : BaseController
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="mediator">MediatR request pipe.</param>
        public CardController(IMediator mediator)
            : base(mediator)
        { }

        /// <summary>
        /// Get a shuffled deck of cards for the given game type.
        /// </summary>
        /// <param name="gameType">Game type <see cref="GamesEnum"/></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetShuffledCards(int gameType)
        {
            var query = new GetShuffledCardsQuery
            {
                GameType = gameType
            };

            return Ok(await Mediator.Send(query));
        }
    }
}
