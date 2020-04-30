using System;

namespace Domain.Exceptions
{
    /// <summary>
    /// Exception during lobby.
    /// </summary>
    public class LobbyException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public LobbyException(string message)
            : base(message)
        {

        }
    }
}
