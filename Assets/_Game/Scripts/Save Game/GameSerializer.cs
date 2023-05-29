using System.IO;
using System.Text;
using BayatGames.SaveGameFree.Serializers;
using Newtonsoft.Json;

namespace Game.SaveSystem{
    public class GameSerializer : JsonSerializer, ISaveGameSerializer{
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.None
        };
        
        public void Serialize<T>(T obj, Stream stream, Encoding encoding){
            using var writer = new StreamWriter(stream, encoding);
            string json = JsonConvert.SerializeObject(obj, SerializerSettings);
            writer.Write(json);
        }

        public T Deserialize<T>(Stream stream, Encoding encoding){
            using var reader = new StreamReader(stream, encoding);
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), SerializerSettings);
        }
    }
}