using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchemaPal.Services.HelperServices
{
    public class JsonConverter : IJsonConverter
    {
        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public T Deserialize<T>(string jsonString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Deserialize<T>(jsonString, options);
        }
    }

}