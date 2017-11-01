// ReSharper disable once CheckNamespace

namespace Serpent.Common.MessageBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Serpent.Common.MessageBus.BusPublishers;
    using Serpent.Common.MessageBus.Exceptions;
    using Serpent.Common.MessageBus.MessageHandlerChain;
    using Serpent.Common.MessageBus.Models;

    /// <summary>
    /// Extensions for parallel message handler chain publisher
    /// </summary>
    public static class ParallelMessageHandlerChainPublisherExtensions
    {
        /// <summary>
        /// Sets up a message handler chain for the bus publisher
        /// </summary>
        /// <typeparam name="TMessageType">The message type</typeparam>
        /// <param name="options">The options</param>
        /// <param name="configureMessageHandlerChain">The action called to setup the message handler chain</param>
        /// <returns>Bus options</returns>
        public static ConcurrentMessageBusOptions<TMessageType> UseSubscriptionChain<TMessageType>(
            this ConcurrentMessageBusOptions<TMessageType> options,
            Action<MessageHandlerChainBuilder<MessageAndHandler<TMessageType>>, Func<MessageAndHandler<TMessageType>, CancellationToken, Task>> configureMessageHandlerChain)
        {
            var builder = new MessageHandlerChainBuilder<MessageAndHandler<TMessageType>>();
            configureMessageHandlerChain(builder, PublishToSubscription.PublishAsync);

            if (builder.HasHandler == false)
            {
                builder.Handler(PublishToSubscription.PublishAsync);
            }

            var subscriptionNotification = new MessageHandlerChainBuildNotification();
            var services = new MessageHandlerChainBuilderSetupServices(subscriptionNotification);
            var chainFunc = builder.BuildFunc(services);
            var newChain = new MessageHandlerChain<MessageAndHandler<TMessageType>>(chainFunc);
            subscriptionNotification.Notify(newChain);

            options.UseCustomPublisher(new ParallelMessageHandlerChainPublisher<TMessageType>(chainFunc));
            return options;
        }

        /// <summary>
        /// Sets up a message handler chain for the bus publisher
        /// </summary>
        /// <typeparam name="TMessageType">The message type</typeparam>
        /// <param name="options">The options</param>
        /// <param name="configureMessageHandlerChain">The action called to setup the message handler chain</param>
        /// <returns>Bus options</returns>
        public static ConcurrentMessageBusOptions<TMessageType> UseSubscriptionChain<TMessageType>(
            this ConcurrentMessageBusOptions<TMessageType> options,
            Action<MessageHandlerChainBuilder<MessageAndHandler<TMessageType>>> configureMessageHandlerChain)
        {
            var builder = new MessageHandlerChainBuilder<MessageAndHandler<TMessageType>>();
            configureMessageHandlerChain(builder);

            if (builder.HasHandler == false)
            {
                builder.Handler(PublishToSubscription.PublishAsync);
            }

            var subscriptionNotification = new MessageHandlerChainBuildNotification();
            var services = new MessageHandlerChainBuilderSetupServices(subscriptionNotification);
            var chainFunc = builder.BuildFunc(services);
            var newChain = new MessageHandlerChain<MessageAndHandler<TMessageType>>(chainFunc);
            subscriptionNotification.Notify(newChain);

            options.UseCustomPublisher(new ParallelMessageHandlerChainPublisher<TMessageType>(chainFunc));
            return options;
        }
    }
}