namespace SchemaPal.DataTransferObjects
{
    public class ExtendedSchemaRecord
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string SchemaJsonFormat { get; set; }

        public DateTime LastSaved { get; set; }
    }
}
