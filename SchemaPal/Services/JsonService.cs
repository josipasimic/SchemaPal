using System.Text.Json;

namespace SchemaPal.Services
{
    public class JsonService : IJsonService
    {
        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public T Deserialize<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }

}
