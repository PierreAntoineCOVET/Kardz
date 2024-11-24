using Domain.Enums;
using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Factories
{
    public static class CardFactory
    {
        public static ICards CreateGameCard(CardsEnum cardsEnum)
        {
            return new CoincheCard(cardsEnum);
        }
    }
}
