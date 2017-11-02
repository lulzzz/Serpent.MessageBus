﻿// ReSharper disable once CheckNamespace

namespace Serpent.MessageBus
{
    using System;
    using System.Collections.Generic;

    using Serpent.MessageBus.MessageHandlerChain.Decorators.Branch;

    /// <summary>
    ///     The branch decorator extensions
    /// </summary>
    public static class BranchExtensions
    {
        /// <summary>
        ///     Branches the message chain into multiple branches, where each has it's own handler or factory
        /// </summary>
        /// <typeparam name="TMessageType">The message type</typeparam>
        /// <param name="messageHandlerChainBuilder">The message handler chain builder</param>
        /// <param name="firstBranch">The first branch</param>
        /// <param name="branches">The other branch(es)</param>
        public static void Branch<TMessageType>(
            this IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder,
            Action<IMessageHandlerChainBuilder<TMessageType>> firstBranch,
            params Action<IMessageHandlerChainBuilder<TMessageType>>[] branches)
        {
            if (firstBranch == null)
            {
                throw new ArgumentNullException(nameof(firstBranch));
            }

            messageHandlerChainBuilder.Handle(
                services =>
                    {
                        var allBranches = new List<Action<IMessageHandlerChainBuilder<TMessageType>>>
                        {
                            firstBranch
                        };

                        allBranches.AddRange(branches);

                        var handler = new BranchHandler<TMessageType>(allBranches);
                        services.BuildNotification.AddNotification(handler.MessageHandlerChainBuilt);
                        return handler.HandleMessageAsync;
                    });
        }
    }
}