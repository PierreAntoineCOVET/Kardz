﻿using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repositories.ReadEntities;
using Repositories.ReadRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Notifications.Game
{
    /// <summary>
    /// Handler for all game events to be saved in the read model.
    /// </summary>
    public class GameEventsHandler :
        INotificationHandler<GameCreatedEvent>
    {
        /// <summary>
        /// Read modele repository.
        /// </summary>
        private readonly IGenericRepository GenericRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="genericRepository">Reand model repository.</param>
        public GameEventsHandler(IGenericRepository genericRepository) 
        {
            GenericRepository = genericRepository;
        }

        /// <summary>
        /// Handler for <see cref="GameCreatedEvent"/>.
        /// </summary>
        /// <param name="notification">Create game notification event.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
        {
            var coincheGame = new CoincheGame
            {
                Id = notification.GameId,
                Teams = notification.Teams.Select(t => new CoincheTeam
                {
                    GameId = notification.GameId,
                    Number = t.Number,
                    Score = 0,
                    Players = t.Players.Select(p => new CoinchePlayer
                    {
                        Id = p.Id,
                        Cards = String.Join(";", p.Cards.Cast<int>()),
                        Number = p.Number,
                        GameId = notification.GameId,
                        TeamNumber = t.Number

                    }).ToList()
                }).ToList()
            };

            await GenericRepository.Add(coincheGame);

            await GenericRepository.SaveChanges();

        }
    }
}