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
    public class CoincheContract : IContract
    {
        /// <summary>
        /// Number of times a contrac has been passed.
        /// </summary>
        private int PassCounter = 0;

        /// <summary>
        /// Player Id currently owning the contract.
        /// </summary>
        private Guid? Owner;

        /// <summary>
        /// Special state of the contract regarding coinche.
        /// </summary>
        private ContractCoincheState CoincheState = ContractCoincheState.NotCoinched;

        /// <summary>
        /// Contract current color. Null if none.
        /// </summary>
        public ColorEnum? Color { get; private set; }

        /// <summary>
        /// Contract current value. Null if none.
        /// </summary>
        public int? Value { get; private set; }

        //private ContractState _CurrentState;
        /// <summary>
        /// Get the current contract state.
        /// </summary>
        public ContractState CurrentState { get; private set; } = ContractState.Valid;

        /// <summary>
        /// Return the state of the contract if we apply the given values.
        /// </summary>
        /// <param name="color">Contract color.</param>
        /// <param name="value">Contract value.</param>
        /// <param name="coinched">Contract coinched state.</param>
        /// <returns></returns>
        public ContractState GetNextState(ColorEnum? color, int? value, bool coinched)
        {
            if(!Color.HasValue && !Value.HasValue
                && !color.HasValue && !value.HasValue && PassCounter == 3)
            {
                return ContractState.Failed;
            }

            var normalClose = Color.HasValue && Value.HasValue 
                && !color.HasValue && !value.HasValue
                && PassCounter == 3;

            var coinchedClose = CoincheState == ContractCoincheState.Coinched
                && !color.HasValue && !value.HasValue && PassCounter == 1;

            var counterCoinchedClose = CoincheState == ContractCoincheState.Coinched
                && coinched;

            if (normalClose || coinchedClose || counterCoinchedClose)
            {
                return ContractState.Closed;
            }

            return ContractState.Valid;
        }

        /// <summary>
        /// Set color and value to the contract.
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        /// <param name="player">Current player id.</param>
        /// <param name="coinched">Contract value</param>
        public ContractMadeEvent GetContractMadeEvent(ColorEnum? color, int? value, Guid player, bool coinched)
        {
            if(!color.HasValue && !value.HasValue)
            {
                if (coinched)
                {
                    return GetCoinchedEvent(player);
                }
                else
                {
                    return GetPassedEvent();
                }
            }
            else
            {
                return GetNewContractEvent(color.Value, value.Value, player);
            }
        }

        private ContractMadeEvent GetNewContractEvent(ColorEnum color, int value, Guid player)
        {
            CheckContract(color, value, player);

            var contractMadeEvent = GetEmptyContract();

            contractMadeEvent.PassCounter = 0;
            contractMadeEvent.Color = color;
            contractMadeEvent.Value = value;
            contractMadeEvent.Owner = player;
            contractMadeEvent.CoincheState = ContractCoincheState.NotCoinched;

            return contractMadeEvent;
        }

        private ContractMadeEvent GetPassedEvent()
        {
            var contractMadeEvent = GetEmptyContract();

            contractMadeEvent.PassCounter = PassCounter + 1;
            contractMadeEvent.Color = Color;
            contractMadeEvent.Value = Value;
            contractMadeEvent.Owner = Owner;
            contractMadeEvent.CoincheState = CoincheState;

            return contractMadeEvent;
        }

        private ContractMadeEvent GetCoinchedEvent(Guid player)
        {
            CheckCoinchableContract();

            var contractMadeEvent = GetEmptyContract();

            contractMadeEvent.PassCounter = 0;
            contractMadeEvent.Color = Color;
            contractMadeEvent.Value = Value;
            contractMadeEvent.Owner = player;
            contractMadeEvent.CoincheState = CoincheState == ContractCoincheState.NotCoinched
                ? ContractCoincheState.Coinched
                : ContractCoincheState.CounterCoinched;

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
        /// Throw GameException if any rules concerning the player making a contract are brokens.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <param name="player"></param>
        /// <exception cref="GameException"></exception>
        private void CheckContract(ColorEnum color, int value, Guid player)
        {
            if (value < 80
                || value % 10 != 0
                || value > 170)
            {
                throw new GameException("Contract value must be between 80 and 160 (or capot).");
            }

            if (Owner != null)
            {
                if (player == Owner
                    && color == Color)
                {
                    throw new GameException("Player must change color to speak over himself.");
                }
            }
        }

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        public void Apply(ContractMadeEvent @event)
        {
            CurrentState = ContractState.Valid;
            PassCounter = @event.PassCounter;
            Color = @event.Color;
            Value = @event.Value;
            Owner = @event.Owner;
            CoincheState = @event.CoincheState;
        }

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        public void Apply(ContractFailedEvent @event)
        {
            CurrentState = ContractState.Failed;
            PassCounter = @event.ContractPassedCount;
        }

        /// <summary>
        /// Return true if the the next player in line should be skipped (his part just coinched).
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        public bool ShouldSkipNextPlayer(ContractMadeEvent @event)
        {
            return @event.CoincheState == ContractCoincheState.Coinched && @event.PassCounter == 1;
        }
    }
}
