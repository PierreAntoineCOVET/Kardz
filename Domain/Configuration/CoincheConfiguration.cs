using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configuration
{
    /// <summary>
    /// Coinche game configuration.
    /// </summary>
    public class CoincheConfiguration
    {
        /// <summary>
        /// Configuration key in appsettings.json.
        /// </summary>
        public const string ConfigurationKey = "CoincheConfiguration";

        /// <summary>
        /// Time limit for each player turn.
        /// </summary>
        public int TimerLengthInSecond { get; set; }

        /// <summary>
        /// Network offset to add server side to account for lag.
        /// </summary>
        public int NetworkOffsetInSecond { get; set; }
    }
}
