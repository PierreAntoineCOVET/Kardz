﻿using Domain.Domain.Implementations;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Interfaces
{
    public interface IGame
    {
        Guid Id { get; }

        IEnumerable<ITeam> Teams { get; }

        void AddPlayers(IEnumerable<Player> players);

        IEnumerable<CardsEnum> GetCardsForPlayer(Guid playerId);
    }
}