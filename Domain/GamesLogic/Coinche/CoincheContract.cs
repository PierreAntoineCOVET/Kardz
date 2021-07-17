using Domain.Enums;
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
        private ColorEnum Color;

        /// <summary>
        /// Minimum valid value.
        /// </summary>
        private int Value;

        /// <summary>
        /// Number of times a contrac has been passed.
        /// </summary>
        private int PassCounter = 0;

        /// <summary>
        /// True if the contract was set at least once.
        /// </summary>
        private bool IsInitialised = false;

        /// <summary>
        /// Return true if the game has to redistribute the players cards
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        /// <returns></returns>
        public bool ForceGameRedistribution(ColorEnum? color, int? value)
        {
            return !color.HasValue && !value.HasValue && PassCounter == 3;
        }

        /// <summary>
        /// Set color and value to the contract.
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        public void SetContract(ColorEnum? color, int? value)
        {
            if(!color.HasValue && !value.HasValue)
            {
                PassCounter++;
            }
            else
            {
                PassCounter = 0;
                Color = color.Value;
                Value = value.Value;
                IsInitialised = true;
            }
        }

        public ColorEnum? GetColor()
        {
            return IsInitialised ? Color : null;
        }

        public int? GetValue()
        {
            return IsInitialised ? Value : null;
        }

        public bool HasLastPlayerPassed()
        {
            return PassCounter > 0;
        }
    }
}
