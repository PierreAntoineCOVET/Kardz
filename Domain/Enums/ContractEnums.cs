using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /// <summary>
    /// Represente the coinched state of the contract.
    /// </summary>
    public enum ContractCoincheState
    {
        NotCoinched,
        Coinched,
        CounterCoinched
    }

    /// <summary>
    /// Represent the overall contract state.
    /// </summary>
    public enum ContractState
    {
        Failed,
        Valid,
        Closed
    }
}
