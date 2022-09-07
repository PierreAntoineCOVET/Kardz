using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using System;

namespace Domain.GamesLogic.Coinche
{
    /// <summary>
    /// Coinche game contract
    /// </summary>
    internal class CoincheContract : IContract
    {
        /// <summary>
        /// Number of times a contrac has been passed.
        /// </summary>
        private int PassCounter = 0;

        /// <summary>
        /// Number of the team owning the contract.
        /// </summary>
        private int? OwningTeamNumber;

        /// <summary>
        /// Special state of the contract regarding coinche.
        /// </summary>
        public ContractCoincheStatesEnum CoincheState { get; private set; } = ContractCoincheStatesEnum.NotCoinched;

        /// <summary>
        /// Contract current color. Null if none.
        /// </summary>
        public CoincheCardColorsEnum? Color { get; private set; }

        /// <summary>
        /// Contract current value. Null if none.
        /// </summary>
        public int? Value { get; private set; }

        /// <summary>
        /// Get the current contract state.
        /// </summary>
        public ContractStatesEnum CurrentState { get; private set; } = ContractStatesEnum.Valid;

        /// <summary>
        /// Update the contract with the given parameters.
        /// </summary>
        /// <param name="color">Contract color.</param>
        /// <param name="value">Contract value.</param>
        /// <param name="coinched">Contract coinched state.</param>
        /// <param name="bettingTeamNumber">Number of the team making the bet.</param>
        /// <returns></returns>
        public ContractChangedEvent Update(CoincheCardColorsEnum? color, int? value, bool coinched, int bettingTeamNumber)
        {
            if (value.HasValue
                && (value < 80
                    || value % 10 != 0
                    || value > 170))
            {
                throw new GameException("Contract value must be a multiple of 10 and between 80 and 160 (or capot).");
            }

            if(coinched && !Color.HasValue && !Value.HasValue)
            {
                throw new GameException($"Cannot coinche empty contract");
            }

            if(coinched && bettingTeamNumber == OwningTeamNumber)
            {
                throw new GameException("Cannot coinche your teammate");
            }

            if (!Color.HasValue && !Value.HasValue
                && !color.HasValue && !value.HasValue && PassCounter == 3)
            {
                return GetContractFailedEvent();
            }

            var normalClose = Color.HasValue && Value.HasValue 
                && !color.HasValue && !value.HasValue
                && PassCounter == 3;

            var coinchedClose = CoincheState == ContractCoincheStatesEnum.Coinched
                && !color.HasValue && !value.HasValue && PassCounter == 1;

            var counterCoinchedClose = CoincheState == ContractCoincheStatesEnum.Coinched
                && coinched;

            if (normalClose || coinchedClose || counterCoinchedClose)
            {
                return GetContractClosedEvent(color, value, bettingTeamNumber, coinched);
            }

            return GetContractUpdatedEvent(color, value, bettingTeamNumber, coinched);
        }

        private ContractChangedEvent GetContractClosedEvent(CoincheCardColorsEnum? color, int? value, int owningTeam, bool coinched)
        {
            var contractChangedEvent = GetEmptyContract();
            contractChangedEvent.PassCounter = 0;
            contractChangedEvent.OwningTeamNumber = owningTeam;
            contractChangedEvent.ContractState = ContractStatesEnum.Closed;

            if (coinched)
            {
                contractChangedEvent.CoincheState = CoincheState == ContractCoincheStatesEnum.NotCoinched
                    ? ContractCoincheStatesEnum.Coinched
                    : ContractCoincheStatesEnum.CounterCoinched;
            }
            else
            {
                contractChangedEvent.CoincheState = CoincheState;
            }
            if(value.HasValue && color.HasValue)
            {
                contractChangedEvent.Color = color;
                contractChangedEvent.Value = value;
            }
            else
            {
                contractChangedEvent.Color = Color;
                contractChangedEvent.Value = Value;
            }

            return contractChangedEvent;
        }

        private ContractChangedEvent GetContractFailedEvent()
        {
            var contractChangedEvent = GetEmptyContract();

            contractChangedEvent.Id = Guid.NewGuid();
            contractChangedEvent.Color = null;
            contractChangedEvent.Value = null;
            contractChangedEvent.OwningTeamNumber = null;
            contractChangedEvent.CoincheState = ContractCoincheStatesEnum.NotCoinched;
            contractChangedEvent.PassCounter = 0;
            contractChangedEvent.ContractState = ContractStatesEnum.Failed;

            return contractChangedEvent;
        }

        private ContractChangedEvent GetContractUpdatedEvent(CoincheCardColorsEnum? color, int? value, int owningTeam, bool coinched)
        {
            ContractChangedEvent contractChangedEvent = null;

            if (!color.HasValue && !value.HasValue)
            {
                if (coinched)
                {
                    contractChangedEvent = GetCoinchedEvent(owningTeam);
                }
                else
                {
                    contractChangedEvent = GetPassedEvent();
                }
            }
            else
            {
                contractChangedEvent = GetNewContractEvent(color.Value, value.Value, owningTeam);
            }

            contractChangedEvent.ContractState = ContractStatesEnum.Valid;

            return contractChangedEvent;
        }

        private ContractChangedEvent GetNewContractEvent(CoincheCardColorsEnum color, int value, int? owningTeam)
        {
            var contractChangedEvent = GetEmptyContract();

            contractChangedEvent.PassCounter = 0;
            contractChangedEvent.Color = color;
            contractChangedEvent.Value = value;
            contractChangedEvent.OwningTeamNumber = owningTeam;
            contractChangedEvent.CoincheState = ContractCoincheStatesEnum.NotCoinched;

            return contractChangedEvent;
        }

        private ContractChangedEvent GetPassedEvent()
        {
            var contractChangedEvent = GetEmptyContract();

            contractChangedEvent.PassCounter = PassCounter + 1;
            contractChangedEvent.Color = Color;
            contractChangedEvent.Value = Value;
            contractChangedEvent.OwningTeamNumber = OwningTeamNumber;
            contractChangedEvent.CoincheState = CoincheState;

            return contractChangedEvent;
        }

        private ContractChangedEvent GetCoinchedEvent(int? owningTeam)
        {
            var contractChangedEvent = GetEmptyContract();

            contractChangedEvent.PassCounter = 0;
            contractChangedEvent.Color = Color;
            contractChangedEvent.Value = Value;
            contractChangedEvent.OwningTeamNumber = owningTeam;
            contractChangedEvent.CoincheState = CoincheState == ContractCoincheStatesEnum.NotCoinched
                ? ContractCoincheStatesEnum.Coinched
                : ContractCoincheStatesEnum.CounterCoinched;

            return contractChangedEvent;
        }

        private ContractChangedEvent GetEmptyContract()
        {
            return new ContractChangedEvent
            {
                Id = Guid.NewGuid()
            };
        }

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        public void Apply(ContractChangedEvent @event)
        {
            CurrentState = ContractStatesEnum.Valid;
            PassCounter = @event.PassCounter;
            Color = @event.Color;
            Value = @event.Value;
            OwningTeamNumber = @event.OwningTeamNumber;
            CoincheState = @event.CoincheState;
        }

        /// <summary>
        /// Return true if the the next player in line should be skipped (his part just coinched).
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        public bool ShouldSkipNextPlayer(ContractChangedEvent @event)
        {
            return @event.CoincheState == ContractCoincheStatesEnum.Coinched && @event.PassCounter == 1;
        }
    }
}
