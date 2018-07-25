using System;
using System.Threading.Tasks;

namespace KDCLLC.BuildingBlocks
{
    /// <summary>
    /// The <see cref="IEventBus"/> provides an abstraction to event bus infrastructure.
    /// </summary>
    public interface IEventBus : IDisposable
    {
        /// <summary>
        /// Subscribe to an event.
        /// </summary>
        /// <typeparam name="T"> T will be an IMessageListener for salesforce.</typeparam>
        /// <param name="eventMessage"></param>
        /// <returns></returns>
        Task Subscribe<T>(BusEvent eventMessage) where T : class;

        /// <summary>
        /// Unsubscribe from an event.
        /// </summary>
        /// <typeparam name="T"> T will be an IMessageListener for salesforce.</typeparam>
        /// <param name="eventMessage">The <see cref="BusEvent"/> to unsubscribe from <see cref="IEventBus"/>.</param>
        /// <returns><see cref="Task"/> void.</returns>
        Task Unsubscribe<T>(BusEvent eventMessage) where T : class;


        /// <summary>
        /// Publish an event to the <see cref="IEventBus"/>.
        /// </summary>
        /// <param name="message">The <see cref="BusEvent"/> to publish to <see cref="IEventBus"/>.</param>
        /// <returns><see cref="Task"/> void.</returns>
        Task Publish(BusEvent message);

        /// <summary>
        /// Publish an event to the <see cref="IEventBus"/>.
        /// </summary>
        /// <param name="message">The <see cref="BusEvent"/> to publish to <see cref="IEventBus"/>.</param>
        /// <returns><see cref="Task"/> void.</returns>
        Task Publish(object message);
    }
}
