namespace SchemaPal.Services
{
    public interface IJsonConverter
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string jsonString);
    }
}
