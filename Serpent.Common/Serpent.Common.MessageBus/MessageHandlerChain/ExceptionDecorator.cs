﻿namespace Serpent.Common.MessageBus.MessageHandlerChain
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ExceptionDecorator<TMessageType> : MessageHandlerChainDecorator<TMessageType>
    {
        private readonly Func<TMessageType, CancellationToken, Task> handlerFunc;

        private readonly Func<TMessageType, Exception, Task<bool>> exceptionHandlerFunc;

        public ExceptionDecorator(Func<TMessageType, CancellationToken, Task> handlerFunc, Func<TMessageType, Exception, Task<bool>> exceptionHandlerFunc)
        {
            this.handlerFunc = handlerFunc;
            this.exceptionHandlerFunc = exceptionHandlerFunc;
        }

        public override async Task HandleMessageAsync(TMessageType message, CancellationToken token)
        {
            try
            {
                await this.handlerFunc(message, token).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                if (await this.exceptionHandlerFunc(message, exception).ConfigureAwait(false))
                {
                    throw;
                }
            }
        }
    }
}