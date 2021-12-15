using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Altinn.TestClient.Models;

namespace Altinn.TestClient.Converters
{
    public class EmbeddedListConverter : JsonConverterFactory
    {
        /// Check that the type is List<Role> or List<Right>
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            if (typeToConvert.GetGenericTypeDefinition() != typeof(List<>))
            {
                return false; 
            }

            Type listType = typeToConvert.GetGenericArguments()[0];
            if (listType != typeof(Right) && listType != typeof(Role))
            {
                return false;
            }

            return true; 
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type listValueType = typeToConvert.GetGenericArguments()[0];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(EmbeddedListConverterInner<>).MakeGenericType(
                    new Type[] { listValueType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null);

            return converter;
        }

        private class EmbeddedListConverterInner<T> : JsonConverter<List<T>>
        {
            private readonly JsonConverter<T> _converter;
            private readonly Type _type;

            public EmbeddedListConverterInner(JsonSerializerOptions options)
            {
                _converter = (JsonConverter<T>)options.GetConverter(typeof(T));
                _type = typeof(T);
            }

            public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (_converter == null)
                {
                    throw new JsonException($"Unable to convert \"{_type}\".");
                }

                var list = new List<T>();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        continue;
                    }
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        return list;
                    }

                    T element = _converter.Read(ref reader, _type, options);
                    list.Add(element);
                }

                throw new JsonException($"End of array not found.");
            }

            public override void Write(Utf8JsonWriter writer, List<T> list, JsonSerializerOptions options)
            {
                if (_converter == null)
                {
                    throw new JsonException($"Unable to convert \"{_type}\".");
                }

                writer.WriteStartArray();
                foreach (T value in list)
                {
                    _converter.Write(writer, value, options);
                }
                writer.WriteEndArray();
            }
        }
    }
}
