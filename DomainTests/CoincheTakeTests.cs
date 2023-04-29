using Domain.Exceptions;
using Domain.GamesLogic.Coinche;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    [TestClass]
    public class CoincheTakeTests
    {
        private IEnumerable<CoincheCard> CardsSetOne = new List<CoincheCard>
        {
            new CoincheCard(Enums.CardsEnum.TenSpade),
            new CoincheCard(Enums.CardsEnum.NineSpade),

            new CoincheCard(Enums.CardsEnum.AsDiamond),
            new CoincheCard(Enums.CardsEnum.QueenDiamond),
            new CoincheCard(Enums.CardsEnum.NineDiamond),

            new CoincheCard(Enums.CardsEnum.JackClub),
            new CoincheCard(Enums.CardsEnum.TenClub),
            new CoincheCard(Enums.CardsEnum.NineClub)
        };

        private IEnumerable<CoincheCard> CardsSetTwo = new List<CoincheCard>
        {
            new CoincheCard(Enums.CardsEnum.EightClub),
            new CoincheCard(Enums.CardsEnum.SevenClub),

            new CoincheCard(Enums.CardsEnum.TenDiamond),
            new CoincheCard(Enums.CardsEnum.SevenDiamond),

            new CoincheCard(Enums.CardsEnum.EightHeart),
            new CoincheCard(Enums.CardsEnum.SevenHeart),

            new CoincheCard(Enums.CardsEnum.EightSpade),
            new CoincheCard(Enums.CardsEnum.SevenSpade)
        };

        private IEnumerable<CoincheCard> CardsSetThree = new List<CoincheCard>
        {
            new CoincheCard(Enums.CardsEnum.EightClub),
            new CoincheCard(Enums.CardsEnum.SevenClub),

            new CoincheCard(Enums.CardsEnum.JackDiamond),
            new CoincheCard(Enums.CardsEnum.SevenDiamond),

            new CoincheCard(Enums.CardsEnum.EightHeart),
            new CoincheCard(Enums.CardsEnum.SevenHeart),

            new CoincheCard(Enums.CardsEnum.EightSpade),
            new CoincheCard(Enums.CardsEnum.SevenSpade)
        };

        [TestMethod]
        public void GetPlayableCards_TurnStart_ReturnAll()
        {
            var take = new CoincheTake();
            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;

            var takeChangedEvent = take.StartNewTake(CardsSetOne, trumpColor);

            CollectionAssert.AreEqual(CardsSetOne.ToList(), takeChangedEvent.NextPlayerAvailableCards.ToList());
        }

        [TestMethod]
        public void GetPlayableCards_AskForNonTrump_CanProvideColor()
        {
            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;
            var take = new CoincheTake();
            take.Apply(new Events.TakeChangedEvent
            {
                CurrentFold = new List<CoincheCard>(),
                PreviousFold = new List<CoincheCard>(),
                NextPlayerAvailableCards = CardsSetTwo
            });
            var expectedCards = new List<CoincheCard>
            {
                new CoincheCard(Enums.CardsEnum.JackClub),
                new CoincheCard(Enums.CardsEnum.TenClub),
                new CoincheCard(Enums.CardsEnum.NineClub)
            };

            var takeChangedEvent = take.Play(new CoincheCard(Enums.CardsEnum.EightClub), CardsSetOne, trumpColor);

            CollectionAssert.AreEqual(expectedCards.ToList(), takeChangedEvent.NextPlayerAvailableCards.ToList());
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void GetPlayableCards_PlayInvalidCard_ThrowGameException()
        {
            var take = new CoincheTake();
            take.Apply(new Events.TakeChangedEvent
            {
                CurrentFold = new List<CoincheCard>(),
                PreviousFold = new List<CoincheCard>(),
                NextPlayerAvailableCards = CardsSetTwo
            });

            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;
            var expectedCards = new List<CoincheCard>
            {
                new CoincheCard(Enums.CardsEnum.AsDiamond),
                new CoincheCard(Enums.CardsEnum.QueenDiamond),
                new CoincheCard(Enums.CardsEnum.NineDiamond),
            };

            _ = take.Play(new CoincheCard(Enums.CardsEnum.AsHeart), CardsSetOne, trumpColor);
        }

        [TestMethod]
        public void GetPlayableCards_AskForNonTrump_ShouldCut()
        {
            var take = new CoincheTake();
            take.Apply(new Events.TakeChangedEvent
            {
                CurrentFold = new List<CoincheCard>(),
                PreviousFold = new List<CoincheCard>(),
                NextPlayerAvailableCards = CardsSetTwo
            });

            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;
            var expectedCards = new List<CoincheCard>
            {
                new CoincheCard(Enums.CardsEnum.AsDiamond),
                new CoincheCard(Enums.CardsEnum.QueenDiamond),
                new CoincheCard(Enums.CardsEnum.NineDiamond),
            };

            var takeChangedEvent = take.Play(new CoincheCard(Enums.CardsEnum.SevenHeart), CardsSetOne, trumpColor);

            CollectionAssert.AreEqual(expectedCards.ToList(), takeChangedEvent.NextPlayerAvailableCards.ToList());
        }

        [TestMethod]
        public void GetPlayableCards_AskForNonTrump_ShouldReCut_HasBestTrump()
        {
            var take = new CoincheTake();
            var currentFold = new List<CoincheCard>();
            currentFold.Add(new CoincheCard(Enums.CardsEnum.AsHeart));
            take.Apply(new Events.TakeChangedEvent
            {
                CurrentFold = currentFold,
                PreviousFold = new List<CoincheCard>(),
                NextPlayerAvailableCards = CardsSetTwo
            });
            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;
            var expectedCards = new List<CoincheCard>
            {
                new CoincheCard(Enums.CardsEnum.AsDiamond),
                new CoincheCard(Enums.CardsEnum.NineDiamond)
            };

            var takeChangedEvent = take.Play(new CoincheCard(Enums.CardsEnum.TenDiamond), CardsSetOne, trumpColor);

            CollectionAssert.AreEqual(expectedCards.ToList(), takeChangedEvent.NextPlayerAvailableCards.ToList());
        }

        [TestMethod]
        public void GetPlayableCards_AskForNonTrump_ShouldReCut_HasWorstTrump()
        {
            var take = new CoincheTake();
            var currentFold = new List<CoincheCard>();
            currentFold.Add(new CoincheCard(Enums.CardsEnum.AsHeart));
            take.Apply(new Events.TakeChangedEvent
            {
                CurrentFold = currentFold,
                PreviousFold = new List<CoincheCard>(),
                NextPlayerAvailableCards = CardsSetThree
            });
            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;
            var expectedCards = new List<CoincheCard>
            {
                new CoincheCard(Enums.CardsEnum.AsDiamond),
                new CoincheCard(Enums.CardsEnum.QueenDiamond),
                new CoincheCard(Enums.CardsEnum.NineDiamond)
            };

            var takeChangedEvent = take.Play(new CoincheCard(Enums.CardsEnum.JackDiamond), CardsSetOne, trumpColor);

            CollectionAssert.AreEqual(expectedCards.ToList(), takeChangedEvent.NextPlayerAvailableCards.ToList());
        }

        [TestMethod]
        public void GetPlayableCards_AskForNonTrump_CanCutOrFold()
        {
            var take = new CoincheTake();
            var currentFold = new List<CoincheCard>();
            currentFold.Add(new CoincheCard(Enums.CardsEnum.AsHeart));
            take.Apply(new Events.TakeChangedEvent
            {
                CurrentFold = currentFold,
                PreviousFold = new List<CoincheCard>(),
                NextPlayerAvailableCards = CardsSetTwo
            });
            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;

            var takeChangedEvent = take.Play(new CoincheCard(Enums.CardsEnum.EightHeart), CardsSetOne, trumpColor);

            CollectionAssert.AreEqual(CardsSetOne.ToList(), takeChangedEvent.NextPlayerAvailableCards.ToList());
        }

        [TestMethod]
        public void GetPlayableCards_AskForTrump_CanProvideColor()
        {
            var take = new CoincheTake();
            take.Apply(new Events.TakeChangedEvent
            {
                CurrentFold = new List<CoincheCard>(),
                PreviousFold = new List<CoincheCard>(),
                NextPlayerAvailableCards = CardsSetTwo
            });

            var trumpColor = Enums.CoincheCardColorsEnum.Diamond;
            var expectedCards = new List<CoincheCard>
            {
                new CoincheCard(Enums.CardsEnum.AsDiamond),
                new CoincheCard(Enums.CardsEnum.QueenDiamond),
                new CoincheCard(Enums.CardsEnum.NineDiamond),
            };

            var takeChangedEvent = take.Play(new CoincheCard(Enums.CardsEnum.SevenDiamond), CardsSetOne, trumpColor);

            CollectionAssert.AreEqual(expectedCards.ToList(), takeChangedEvent.NextPlayerAvailableCards.ToList());
        }
    }
}
