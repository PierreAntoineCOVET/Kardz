using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Check if the contract can be updated with the current parameters.
        /// If it's ok, return the future state of the contract with the given values.
        /// </summary>
        /// <param name="color">Contract color.</param>
        /// <param name="value">Contract value.</param>
        /// <param name="coinched">Contract coinched state.</param>
        /// <param name="bettingTeamNumber">Number of the team making the bet.</param>
        /// <returns></returns>
        public ContractStatesEnum ValidateContract(CoincheCardColorsEnum? color, int? value, bool coinched, int bettingTeamNumber)
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
                return ContractStatesEnum.Failed;
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
                return ContractStatesEnum.Closed;
            }

            return ContractStatesEnum.Valid;
        }

        /// <summary>
        /// Set color and value to the contract.
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        /// <param name="owningTeam">Number of the team making the bet.</param>
        /// <param name="coinched">Contract value</param>
        public ContractMadeEvent GetContractMadeEvent(CoincheCardColorsEnum? color, int? value, int? owningTeam, bool coinched)
        {
            if(!color.HasValue && !value.HasValue)
            {
                if (coinched)
                {
                    return GetCoinchedEvent(owningTeam);
                }
                else
                {
                    return GetPassedEvent();
                }
            }
            else
            {
                return GetNewContractEvent(color.Value, value.Value, owningTeam);
            }
        }

        private ContractMadeEvent GetNewContractEvent(CoincheCardColorsEnum color, int value, int? owningTeam)
        {
            var contractMadeEvent = GetEmptyContract();

            contractMadeEvent.PassCounter = 0;
            contractMadeEvent.Color = color;
            contractMadeEvent.Value = value;
            contractMadeEvent.OwningTeamNumber = owningTeam;
            contractMadeEvent.CoincheState = ContractCoincheStatesEnum.NotCoinched;

            return contractMadeEvent;
        }

        private ContractMadeEvent GetPassedEvent()
        {
            var contractMadeEvent = GetEmptyContract();

            contractMadeEvent.PassCounter = PassCounter + 1;
            contractMadeEvent.Color = Color;
            contractMadeEvent.Value = Value;
            contractMadeEvent.OwningTeamNumber = OwningTeamNumber;
            contractMadeEvent.CoincheState = CoincheState;

            return contractMadeEvent;
        }

        private ContractMadeEvent GetCoinchedEvent(int? owningTeam)
        {
            CheckCoinchableContract();

            var contractMadeEvent = GetEmptyContract();

            contractMadeEvent.PassCounter = 0;
            contractMadeEvent.Color = Color;
            contractMadeEvent.Value = Value;
            contractMadeEvent.OwningTeamNumber = owningTeam;
            contractMadeEvent.CoincheState = CoincheState == ContractCoincheStatesEnum.NotCoinched
                ? ContractCoincheStatesEnum.Coinched
                : ContractCoincheStatesEnum.CounterCoinched;

            return contractMadeEvent;
        }

        private ContractMadeEvent GetEmptyContract()
        {
            return new ContractMadeEvent
            {
                Id = Guid.NewGuid()
            };
        }

        private void CheckCoinchableContract()
        {
            if(!Value.HasValue || !Color.HasValue)
            {
                throw new GameException("Cannnot coinche contract without color and value.");
            }
        }

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        public void Apply(ContractMadeEvent @event)
        {
            CurrentState = ContractStatesEnum.Valid;
            PassCounter = @event.PassCounter;
            Color = @event.Color;
            Value = @event.Value;
            OwningTeamNumber = @event.OwningTeamNumber;
            CoincheState = @event.CoincheState;
        }

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        public void Apply(ContractFailedEvent @event)
        {
            CurrentState = ContractStatesEnum.Failed;
            PassCounter = @event.ContractPassedCount;
        }

        /// <summary>
        /// Return true if the the next player in line should be skipped (his part just coinched).
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        public bool ShouldSkipNextPlayer(ContractMadeEvent @event)
        {
            return @event.CoincheState == ContractCoincheStatesEnum.Coinched && @event.PassCounter == 1;
        }
    }
}
