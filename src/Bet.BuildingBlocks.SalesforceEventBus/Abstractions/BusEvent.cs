using Newtonsoft.Json;

namespace Bet.BuildingBlocks.Abstractions
{
    /// <summary>
    /// Container for the Salesforce Messages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusEvent<T>
    {
        /// <summary>
        /// Specific event name for the endpoint pipeline.
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        public T Data { get; set; }
    }
}
