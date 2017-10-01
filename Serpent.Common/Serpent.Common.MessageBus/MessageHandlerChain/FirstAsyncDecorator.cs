﻿namespace Serpent.Common.MessageBus.MessageHandlerChain
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class FirstAsyncDecorator<TMessageType> : MessageHandlerChainDecorator<TMessageType>
    {
        private readonly Func<TMessageType, CancellationToken, Task> handlerFunc;

        private readonly Func<TMessageType, Task<bool>> asyncPredicate;

        private int wasReceived;

        public FirstAsyncDecorator(Func<TMessageType, CancellationToken,  Task> handlerFunc, Func<TMessageType, Task<bool>> asyncPredicate)
        {
            this.handlerFunc = handlerFunc;
            this.asyncPredicate = asyncPredicate;
        }

        public override async Task HandleMessageAsync(TMessageType message, CancellationToken token)
        {
            if (this.wasReceived == 0)
            {
                if (await this.asyncPredicate(message).ConfigureAwait(false))
                {
                    if (Interlocked.CompareExchange(ref this.wasReceived, 1, 0) == 0)
                    {
                        await this.handlerFunc(message, token).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}