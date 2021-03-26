using System;

namespace Domain.Tools.Serialization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InterfaceResolverAttribute : Attribute
    {
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
