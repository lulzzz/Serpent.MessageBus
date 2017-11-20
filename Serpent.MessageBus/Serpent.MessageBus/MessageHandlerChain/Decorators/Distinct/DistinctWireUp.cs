﻿namespace Serpent.MessageBus.MessageHandlerChain.Decorators.Distinct
{
    using System.Linq;
    using System.Reflection;

    using Serpent.MessageBus.MessageHandlerChain.WireUp;

    internal class DistinctWireUp : BaseWireUp<DistinctAttribute, DistinctConfiguration>
    {
        internal const string WireUpExtensionName = "DistinctWireUp";

        private static readonly MethodInfo DistinctMethodInfo = typeof(DistinctExtensions).GetMethods()
            .FirstOrDefault(
                m => m.IsGenericMethodDefinition && m.IsStatic && m.GetCustomAttributes<ExtensionMethodSelectorAttribute>().Any(a => a.Identifier == WireUpExtensionName));

        protected override DistinctConfiguration CreateAndParseConfigurationFromDefaultValue(string text)
        {
            return new DistinctConfiguration
                       {
                           PropertyName = text
                       };
        }

        protected override void WireUpFromAttribute<TMessageType, THandlerType>(
            DistinctAttribute attribute,
            IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder,
            THandlerType handler)
        {
            if (string.IsNullOrWhiteSpace(attribute.PropertyName))
            {
                messageHandlerChainBuilder.Distinct(msg => msg);
                return;
            }

            SelectorSetup<TMessageType>
                .WireUp(
                    attribute.PropertyName,
                        DistinctMethodInfo,
                    (typedMethodInfo, selector) => typedMethodInfo.Invoke(null, new object[] { messageHandlerChainBuilder, selector }));
        }

        protected override void WireUpFromConfiguration<TMessageType, THandlerType>(DistinctConfiguration configuration, IMessageHandlerChainBuilder<TMessageType> messageHandlerChainBuilder, THandlerType handler)
        {
            if (string.IsNullOrWhiteSpace(configuration.PropertyName))
            {
                messageHandlerChainBuilder.Distinct(msg => msg.ToString());
                return;
            }

            SelectorSetup<TMessageType>
                .WireUp(
                    configuration.PropertyName,
                    DistinctMethodInfo,
                    (methodInfo, selector) => methodInfo.Invoke(null, new object[] { messageHandlerChainBuilder, selector }));
        }
    }
}