namespace DTOs.Enums
{
    /// <summary>
    /// Represente the coinched state of the contract.
    /// </summary>
    public enum ContractCoincheStatesEnum
    {
        NotCoinched,
        Coinched,
        CounterCoinched
    }

    /// <summary>
    /// Represent the overall contract state.
    /// </summary>
    public enum ContractStatesEnum
    {
        Failed,
        Valid,
        Closed
    }
}
