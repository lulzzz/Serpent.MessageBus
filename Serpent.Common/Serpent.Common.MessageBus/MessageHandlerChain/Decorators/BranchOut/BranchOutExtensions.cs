﻿// ReSharper disable once CheckNamespace
namespace Serpent.Common.MessageBus
{
    using System;
    using System.Threading.Tasks;

    using Serpent.Common.MessageBus.MessageHandlerChain.Decorators.BranchOut;

    /// <summary>
    /// The branch out extensions
    /// </summary>
    public static class BranchOutExtensions
    {
        /// <summary>
        /// Branch Out one or more parallel branches, running in parallel with the main message handler
        /// </summary>
        /// <typeparam name="TMessageType">The message type</typeparam>
        /// <param name="messageHandlerChainBuilder">The message handler chain builder</param>
        /// <param name="branches">The branches</param>
        /// <returns>The message handler chain builder</returns>
        public static IMessageHandlerChainBuilder<TMessageType> BranchOut<TMessageType>(
            this IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder,
            params Action<IMessageHandlerChainBuilder<TMessageType>>[] branches)
        {
            return messageHandlerChainBuilder.Decorate(
                (innerHandler, services) =>
                    {
                        var handler = new BranchOutDecorator<TMessageType>(branches);
                        services.BuildNotification.AddNotification(handler.MessageHandlerChainBuilt);
                        return (message, token) => Task.WhenAll(handler.HandleMessageAsync(message, token), innerHandler(message, token));
                    });
        }
    }
}