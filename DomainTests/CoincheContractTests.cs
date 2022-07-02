using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.GamesLogic.Coinche;

namespace DomainTests
{
    [TestClass]
    public class CoincheContractTests
    {
        [TestMethod]
        public void ValidateContract_OnePass_IsValid()
        {
            var coincheContract = new CoincheContract();

            var finalState = coincheContract.ValidateContract(null, null, false, 0);

            Assert.AreEqual(ContractStatesEnum.Valid, finalState);
        }

        [TestMethod]
        public void ValidateContract_OneBet_IsValid()
        {
            var coincheContract = new CoincheContract();

            var finalState = coincheContract.ValidateContract(CoincheCardColorsEnum.Diamond, 80, false, 0);

            Assert.AreEqual(ContractStatesEnum.Valid, finalState);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_OneBetWrongLowValue_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            _ = coincheContract.ValidateContract(CoincheCardColorsEnum.Diamond, 70, false, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_OneBetWrongHighValue_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            var finalState = coincheContract.ValidateContract(CoincheCardColorsEnum.Diamond, 180, false, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_OneBetWrongValue_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            _ = coincheContract.ValidateContract(CoincheCardColorsEnum.Diamond, 87, false, 0);
        }

        [TestMethod]
        public void ValidateContract_AllPassed_ReturnFailed()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractMadeEvent
            {
                CoincheState = ContractCoincheStatesEnum.NotCoinched,
                PassCounter = 3
            });

            var finalState = coincheContract.ValidateContract(null, null, false, 1);

            Assert.AreEqual(ContractStatesEnum.Failed, finalState);
        }

        [TestMethod]
        public void ValidateContract_OneBetAllPass_ReturnClosed()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractMadeEvent
            {
                CoincheState = ContractCoincheStatesEnum.NotCoinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 3,
                Value = 100
            });

            var finalState = coincheContract.ValidateContract(null, null, false, 1);

            Assert.AreEqual(ContractStatesEnum.Closed, finalState);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_CoincheEmptyContract_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            _ = coincheContract.ValidateContract(null, null, true, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_CoincheYourTeammate_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            coincheContract.Apply(new ContractMadeEvent
            {
                CoincheState = ContractCoincheStatesEnum.Coinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 0,
                Value = 100,
                OwningTeamNumber = 0
            });

            _ = coincheContract.ValidateContract(null, null, true, 0);
        }

        [TestMethod]
        public void ValidateContract_Coinche_IsValid()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractMadeEvent
            {
                CoincheState = ContractCoincheStatesEnum.NotCoinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 0,
                Value = 100,
                OwningTeamNumber = 1
            });

            var finalState = coincheContract.ValidateContract(null, null, true, 0);

            Assert.AreEqual(ContractStatesEnum.Valid, finalState);
        }

        [TestMethod]
        public void ValidateContract_CoincheConterCoinche_ReturnsClosed()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractMadeEvent
            {
                CoincheState = ContractCoincheStatesEnum.Coinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 0,
                Value = 100,
                OwningTeamNumber = 1
            });

            var finalState = coincheContract.ValidateContract(null, null, true, 0);

            Assert.AreEqual(ContractStatesEnum.Closed, finalState);
        }
    }
}