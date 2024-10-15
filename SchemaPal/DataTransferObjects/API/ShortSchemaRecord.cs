namespace SchemaPal.DataTransferObjects.API
{
    public class ShortSchemaRecord
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime LastSaved { get; set; }
    }
}
