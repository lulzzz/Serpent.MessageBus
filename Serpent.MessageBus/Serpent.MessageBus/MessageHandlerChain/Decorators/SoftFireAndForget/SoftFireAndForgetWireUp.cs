﻿namespace Serpent.MessageBus.MessageHandlerChain.Decorators.SoftFireAndForget
{
    using Serpent.MessageBus.MessageHandlerChain.WireUp;

    internal class SoftFireAndForgetWireUp : BaseWireUp<SoftFireAndForgetAttribute, SoftFireAndForgetConfiguration>
    {
        protected override SoftFireAndForgetConfiguration CreateAndParseConfigurationFromDefaultValue(string text)
        {
            return new SoftFireAndForgetConfiguration();
        }

        protected override void WireUpFromAttribute<TMessageType, THandlerType>(SoftFireAndForgetAttribute attribute, IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder, THandlerType handler)
        {
            messageHandlerChainBuilder.SoftFireAndForget();
        }

        protected override void WireUpFromConfiguration<TMessageType, THandlerType>(
            SoftFireAndForgetConfiguration configuration,
            IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder,
            THandlerType handler)
        {
            messageHandlerChainBuilder.SoftFireAndForget();
        }
    }
}