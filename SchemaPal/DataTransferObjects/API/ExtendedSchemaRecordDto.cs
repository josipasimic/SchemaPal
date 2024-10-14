namespace SchemaPal.DataTransferObjects.API
{
    public class ExtendedSchemaRecordDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SchemaInJsonFormat { get; set; }

        public ExtendedSchemaRecordDto()
        {
            Name = string.Empty;
            SchemaInJsonFormat = string.Empty;
        }
    }
}
