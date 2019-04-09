namespace Bet.BuildingBlocks.Abstractions
{
    /// <summary>
    /// Salesforce Platform Event.
    /// </summary>
    public class PlatformEvent<T> : BusEvent<T>
    {
        /// <summary>
        /// Specifies Replay Id for the Salesforce.
        /// </summary>
        public int ReplayId { get; set; }
    }
}
