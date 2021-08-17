using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TiledLib
{

    public class TilesetConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(ITileset);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var result = new Tileset();
            result.Source = jo["source"].ToString();

            serializer.Populate(jo.CreateReader(), result);
            return result;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}