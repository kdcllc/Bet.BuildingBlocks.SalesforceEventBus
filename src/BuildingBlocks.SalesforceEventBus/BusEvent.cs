using Newtonsoft.Json;

namespace KDCLLC.BuildingBlocks
{
    /// <summary>
    /// Container for the Salesforce Messages
    /// </summary>
    public class BusEvent
    {

        /// <summary>
        /// Specific event name for the endpoint pipeline.
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }
    }
}
