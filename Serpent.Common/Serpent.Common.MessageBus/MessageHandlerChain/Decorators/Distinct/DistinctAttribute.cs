﻿// ReSharper disable once CheckNamespace
namespace Serpent.Common.MessageBus
{
    using Serpent.Common.MessageBus.MessageHandlerChain.WireUp;

    /// <summary>
    /// The distinct attribute
    /// </summary>
    public class DistinctAttribute : WireUpAttribute
    {
        /// <summary>
        /// Ensures a message of a certain key only passes once
        /// </summary>
        /// <param name="propertyName">The property name</param>
        public DistinctAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public DistinctAttribute()
        {
        }

        /// <summary>
        /// The property name
        /// </summary>
        public string PropertyName { get; }
    }
}