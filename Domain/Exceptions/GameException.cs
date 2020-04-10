using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    /// <summary>
    /// Exception during the game.
    /// </summary>
    public class GameException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public GameException(string message)
            : base(message)
        {

        }
    }
}
