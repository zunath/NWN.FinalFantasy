using System;

namespace NWN.FinalFantasy.Core.Messaging.Contracts
{
    /// <summary>
    /// An implementation of the <c>Event Aggregator</c> pattern.
    /// </summary>
    internal interface IMessageHub : IDisposable
    {
        /// <summary>
        /// Registers a callback which is invoked on every message published by the <see cref="IMessageHub"/>.
        /// <remarks>Invoking this method with a new <paramref name="onMessage"/>overwrites the previous one.</remarks>
        /// </summary>
        /// <param name="onMessage">
        /// The callback to invoke on every message
        /// <remarks>The callback receives the type of the message and the message as arguments</remarks>
        /// </param>
        void RegisterGlobalHandler(Action<Type, object> onMessage);

        /// <summary>
        /// Invoked if an error occurs when publishing a message to a subscriber.
        /// <remarks>Invoking this method with a new <paramref name="onError"/>overwrites the previous one.</remarks>
        /// </summary>
        void RegisterGlobalErrorHandler(Action<Guid, Exception> onError);

        /// <summary>
        /// Publishes the <paramref name="message"/> on the <see cref="IMessageHub"/>.
        /// </summary>
        /// <param name="message">The message to published</param>
        void Publish<T>(T message);

    }
}
