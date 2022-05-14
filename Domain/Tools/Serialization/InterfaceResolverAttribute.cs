using System;

namespace Domain.Tools.Serialization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InterfaceResolverAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateType"></param>
        /// <param name="eventType"></param>
        /// <param name="converter"></param>
        public InterfaceResolverAttribute(Type aggregateType, Type eventType, Type converter)
        {
            Aggregate = aggregateType;
            Event = eventType;
            Converter = converter;
        }

        public Type Aggregate { get; private set; }

        public Type Event { get; private set; }

        public Type Converter { get; private set; }
    }
}
