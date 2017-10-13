﻿namespace Serpent.Common.MessageBus.MessageHandlerChain.Decorators.Delay
{
    using System;

    using Serpent.Common.MessageBus.MessageHandlerChain.WireUp;

    public class DelayWireUp : BaseWireUp<DelayAttribute, DelayConfiguration>
    {
        protected override DelayConfiguration CreateAndParseConfigurationFromDefaultValue(string text)
        {
            if (TimeSpan.TryParse(text, out var delay))
            {
                return new DelayConfiguration
                           {
                               Delay = delay
                           };
            }

            throw new Exception("Delay: Could not parse text to timespan: " + text);
        }

        protected override void WireUpFromAttribute<TMessageType, THandlerType>(
            DelayAttribute attribute,
            IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder,
            THandlerType handler)
        {
            messageHandlerChainBuilder.Delay(attribute.Delay);
        }

        protected override void WireUpFromConfiguration<TMessageType, THandlerType>(
            DelayConfiguration configuration,
            IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder,
            THandlerType handler)
        {
            messageHandlerChainBuilder.Delay(configuration.Delay);
        }
    }
}