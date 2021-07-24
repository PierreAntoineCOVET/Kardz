using DTOs.Shared;
using MediatR;
using System;

namespace EventHandlers.Commands.SetGameContract
{
    /// <summary>
    /// Game contract to set.
    /// </summary>
    public class SetGameContractCommand : IRequest<GameContractDto>
    {
        /// <summary>
        /// Contract color (null if passed).
        /// </summary>
        public int? Color { get; set; }

        /// <summary>
        /// Contract value (null if passed).
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Better (player).
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Game.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Coinched or counter coinched. Null if passed.
        /// </summary>
        public bool? Coinched { get; set; }
    }
}
