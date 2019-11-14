using CometD.NetCore.Salesforce.Messaging;

namespace Bet.Salesforce.TestApp.EventBus.Messages
{
    /// <summary>
    /// The <see cref="CustomMessageEnvelope"/> receives by <see cref="CustomMessageListener"/>.
    /// </summary>
    public class CustomMessageEnvelope : MessageEnvelope<CustomMessagePayload>
    {
    }
}
