using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game.SaveSystem.CustomConverters{
    public class DictionaryConverter : JsonConverter{
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Perform custom deserialization logic here
            // In this example, we assume the input is always a dictionary
            if (reader.TokenType == JsonToken.StartObject)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string key = (string)reader.Value;
                        reader.Read();
                        object value = serializer.Deserialize(reader);
                        dictionary.Add(key, value);
                    }
                }
                return dictionary;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Perform custom serialization logic here
            // In this example, we assume the input is always a dictionary
            Dictionary<string, object> dictionary = (Dictionary<string, object>)value;
            writer.WriteStartObject();
            foreach (var kvp in dictionary)
            {
                writer.WritePropertyName(kvp.Key);
                serializer.Serialize(writer, kvp.Value);
            }
            writer.WriteEndObject();
        }
    }
}