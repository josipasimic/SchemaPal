namespace SchemaPal.DataTransferObjects.API
{
    public class ShortSchemaRecordDto
    {
        public int SchemaId { get; set; }

        public string SchemaName { get; set; }

        public ShortSchemaRecordDto()
        {
            SchemaName = string.Empty;
        }
    }
}
