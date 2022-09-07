using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.GamesLogic.Coinche;

namespace Domain.Tests
{
    [TestClass]
    public class CoincheContractTests
    {
        [TestMethod]
        public void ValidateContract_OnePass_IsValid()
        {
            var coincheContract = new CoincheContract();

            var contractChangedEvent = coincheContract.Update(null, null, false, 0);

            Assert.AreEqual(ContractStatesEnum.Valid, contractChangedEvent.ContractState);
            Assert.AreEqual(ContractCoincheStatesEnum.NotCoinched, contractChangedEvent.CoincheState);
            Assert.AreEqual(null, contractChangedEvent.Value);
            Assert.AreEqual(null, contractChangedEvent.Color);
        }

        [TestMethod]
        public void ValidateContract_OneBet_IsValid()
        {
            var coincheContract = new CoincheContract();

            var contractChangedEvent = coincheContract.Update(CoincheCardColorsEnum.Diamond, 80, false, 0);

            Assert.AreEqual(ContractStatesEnum.Valid, contractChangedEvent.ContractState);
            Assert.AreEqual(ContractCoincheStatesEnum.NotCoinched, contractChangedEvent.CoincheState);
            Assert.AreEqual(80, contractChangedEvent.Value);
            Assert.AreEqual(CoincheCardColorsEnum.Diamond, contractChangedEvent.Color);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_OneBetWrongLowValue_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            _ = coincheContract.Update(CoincheCardColorsEnum.Diamond, 70, false, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_OneBetWrongHighValue_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            _ = coincheContract.Update(CoincheCardColorsEnum.Diamond, 180, false, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_OneBetWrongValue_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            _ = coincheContract.Update(CoincheCardColorsEnum.Diamond, 87, false, 0);
        }

        [TestMethod]
        public void ValidateContract_AllPassed_ReturnFailed()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractChangedEvent
            {
                CoincheState = ContractCoincheStatesEnum.NotCoinched,
                PassCounter = 3
            });

            var contractChangedEvent = coincheContract.Update(null, null, false, 1);

            Assert.AreEqual(ContractStatesEnum.Failed, contractChangedEvent.ContractState);
            Assert.AreEqual(ContractCoincheStatesEnum.NotCoinched, contractChangedEvent.CoincheState);
            Assert.AreEqual(null, contractChangedEvent.Value);
            Assert.AreEqual(null, contractChangedEvent.Color);
            Assert.AreEqual(0, contractChangedEvent.PassCounter);
        }

        [TestMethod]
        public void ValidateContract_OneBetAllPass_ReturnClosed()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractChangedEvent
            {
                CoincheState = ContractCoincheStatesEnum.NotCoinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 3,
                Value = 100
            });

            var contractChangedEvent = coincheContract.Update(null, null, false, 1);

            Assert.AreEqual(ContractStatesEnum.Closed, contractChangedEvent.ContractState);
            Assert.AreEqual(ContractCoincheStatesEnum.NotCoinched, contractChangedEvent.CoincheState);
            Assert.AreEqual(100, contractChangedEvent.Value);
            Assert.AreEqual(CoincheCardColorsEnum.Diamond, contractChangedEvent.Color);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_CoincheEmptyContract_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            _ = coincheContract.Update(null, null, true, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(GameException))]
        public void ValidateContract_CoincheYourTeammate_ThrowGameException()
        {
            var coincheContract = new CoincheContract();

            coincheContract.Apply(new ContractChangedEvent
            {
                CoincheState = ContractCoincheStatesEnum.Coinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 0,
                Value = 100,
                OwningTeamNumber = 0
            });

            _ = coincheContract.Update(null, null, true, 0);
        }

        [TestMethod]
        public void ValidateContract_Coinche_IsValid()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractChangedEvent
            {
                CoincheState = ContractCoincheStatesEnum.NotCoinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 0,
                Value = 100,
                OwningTeamNumber = 1
            });

            var contractChangedEvent = coincheContract.Update(null, null, true, 0);

            Assert.AreEqual(ContractStatesEnum.Valid, contractChangedEvent.ContractState);
            Assert.AreEqual(ContractCoincheStatesEnum.Coinched, contractChangedEvent.CoincheState);
            Assert.AreEqual(100, contractChangedEvent.Value);
            Assert.AreEqual(CoincheCardColorsEnum.Diamond, contractChangedEvent.Color);
        }

        [TestMethod]
        public void ValidateContract_CoincheConterCoinche_ReturnsClosed()
        {
            var coincheContract = new CoincheContract();
            coincheContract.Apply(new ContractChangedEvent
            {
                CoincheState = ContractCoincheStatesEnum.Coinched,
                Color = CoincheCardColorsEnum.Diamond,
                PassCounter = 0,
                Value = 100,
                OwningTeamNumber = 1
            });

            var contractChangedEvent = coincheContract.Update(null, null, true, 0);

            Assert.AreEqual(ContractStatesEnum.Closed, contractChangedEvent.ContractState);
            Assert.AreEqual(ContractCoincheStatesEnum.CounterCoinched, contractChangedEvent.CoincheState);
            Assert.AreEqual(100, contractChangedEvent.Value);
            Assert.AreEqual(CoincheCardColorsEnum.Diamond, contractChangedEvent.Color);
        }
    }
}