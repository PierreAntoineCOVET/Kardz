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
        public ContractMadeEvent GetContractMadeEvent(ColorEnum? color, int? value)
        {
            var contractMadeEvent = new ContractMadeEvent
            {
                Id = Guid.NewGuid()
            };

            if(!color.HasValue && !value.HasValue)
            {
                contractMadeEvent.PassCounter = PassCounter + 1;
            }
            else
            {
                contractMadeEvent.PassCounter = 0;
                contractMadeEvent.Color = color.Value;
                contractMadeEvent.Value = value.Value;
            }

            return contractMadeEvent;
        }

        public ColorEnum? Color => _Color;

        public int? Value => _Value;

        public void Apply(ContractMadeEvent @event)
        {
            PassCounter = @event.PassCounter;
            _Color = @event.Color;
            _Value = @event.Value;
        }
    }
}
