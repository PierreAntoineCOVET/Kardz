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
        /// Card trump color.
        /// </summary>
        private ColorEnum? _Color;

        /// <summary>
        /// Minimum valid value.
        /// </summary>
        private int? _Value;

        /// <summary>
        /// Number of times a contrac has been passed.
        /// </summary>
        private int PassCounter = 0;

        /// <summary>
        /// Player Id currently owning the contract.
        /// </summary>
        private Guid? Owner;

        /// <summary>
        /// Return true if the game has to redistribute the players cards
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        /// <returns></returns>
        public bool IsContractFailed(ColorEnum? color, int? value)
        {
            return !color.HasValue && !value.HasValue && PassCounter == 3;
        }

        /// <summary>
        /// Set color and value to the contract.
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        public ContractMadeEvent GetContractMadeEvent(ColorEnum? color, int? value, Guid player)
        {
            var contractMadeEvent = new ContractMadeEvent
            {
                Id = Guid.NewGuid()
            };

            if(!color.HasValue && !value.HasValue)
            {
                contractMadeEvent.PassCounter = PassCounter + 1;
                contractMadeEvent.Color = _Color;
                contractMadeEvent.Value = _Value;
                contractMadeEvent.Owner = Owner;
            }
            else
            {
                CheckContract(color.Value, value.Value, player);

                contractMadeEvent.PassCounter = 0;
                contractMadeEvent.Color = color.Value;
                contractMadeEvent.Value = value.Value;
                contractMadeEvent.Owner = player;
            }

            return contractMadeEvent;
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

        public ColorEnum? Color => _Color;

        public int? Value => _Value;

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        public void Apply(ContractMadeEvent @event)
        {
            PassCounter = @event.PassCounter;
            _Color = @event.Color;
            _Value = @event.Value;
            Owner = @event.Owner;
        }

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        public void Apply(ContractFailedEvent @event)
        {
            PassCounter = @event.ContractPassedCount;
        }
    }
}
