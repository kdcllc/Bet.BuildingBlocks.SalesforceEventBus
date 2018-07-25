namespace KDCLLC.BuildingBlocks
{
    /// <summary>
    /// Salesforce Platform Event.
    /// </summary>
    public class PlatformEvent : BusEvent
    {
        /// <summary>
        /// Specifies Replay Id for the Salesforce.
        /// </summary>
        public int ReplayId { get; set; }
    }
}
