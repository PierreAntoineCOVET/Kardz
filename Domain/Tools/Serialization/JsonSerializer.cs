using Domain.Entities.EventStoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Tools.Serialization
{
    /// <summary>
    /// Json parsing with mapping from domain interface to concerte type.
    /// </summary>
    public class JsonSerializer
    {
        private static IDictionary<(string aggregate, string @event), IEnumerable<JsonConverter>> Converters = new Dictionary<(string aggregate, string @event), IEnumerable<JsonConverter>>();

        /// <summary>
        /// Register a concerte type to use when serializing / deserializing interfaces.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="converterType">Type of the mapper.</param>
        public static void Register(Type aggregateType, Type eventType, Type converterType)
        {
            Converters.TryGetValue((aggregateType.FullName, eventType.FullName), out IEnumerable<JsonConverter> registeredConverters);

            if (registeredConverters?.Any() == true)
            {
                if(registeredConverters.Any(c => c.GetType() == converterType))
                {
                    throw new InvalidOperationException($"Multiple registration of converters {converterType.FullName} for key ({aggregateType.FullName}, {eventType.FullName})");
                }
                ((List<JsonConverter>)registeredConverters).Add((JsonConverter)Activator.CreateInstance(converterType));
            }
            else
            {
                Converters.Add((aggregateType.FullName, eventType.FullName), new List<JsonConverter> { (JsonConverter)Activator.CreateInstance(converterType) });
            }
        }

        /// <summary>
        /// Serialize object (as concrete type) to Json.
        /// </summary>
        /// <typeparam name="T">Type to serialize.</typeparam>
        /// <param name="object">Object to serialize.</param>
        /// <returns></returns>
        public string Serialize<T>(string typeAggregate, string typeEvent, T @object)
        {
            return System.Text.Json.JsonSerializer.Serialize(@object, GetOptions(typeAggregate, typeEvent));
        }

        /// <summary>
        /// Deserialize type as @event.Type.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public object Deserialize(string typeAggregate, Event @event)
        {
            return System.Text.Json.JsonSerializer.Deserialize(@event.Datas, Type.GetType(@event.Type), GetOptions(typeAggregate, @event.Type));
        }

        /// <summary>
        /// Return the mapping options of serialization / deserialization with interfaces.
        /// </summary>
        /// <param name="aggregateType">Type (fullname) of the aggregate.</param>
        /// <param name="eventType">Type (fullname) of the event.</param>
        /// <returns></returns>
        private JsonSerializerOptions GetOptions(string aggregateType, string eventType)
        {
            var options = new JsonSerializerOptions();
            Converters.TryGetValue((aggregateType, eventType), out IEnumerable<JsonConverter> converters);

            if(converters == null)
            {
                return options;
            }

            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }

            return options;
        }
    }
}
