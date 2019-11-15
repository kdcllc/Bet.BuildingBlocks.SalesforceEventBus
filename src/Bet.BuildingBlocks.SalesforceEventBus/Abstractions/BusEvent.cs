using Newtonsoft.Json;

namespace Bet.BuildingBlocks.Abstractions
{
    /// <summary>
    /// Container for the Salesforce Messages.
    /// </summary>
    public class BusEvent<T>
    {
        /// <summary>
        /// Specific event name for the endpoint pipeline.
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }

        public T Data { get; set; }
    }
}
