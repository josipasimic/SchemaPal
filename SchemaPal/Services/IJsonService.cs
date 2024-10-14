namespace SchemaPal.Services
{
    public interface IJsonService
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string jsonString);
    }
}
