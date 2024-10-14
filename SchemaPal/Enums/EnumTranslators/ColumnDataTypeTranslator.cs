namespace SchemaPal.Enums.EnumTranslators
{
    public static class ColumnDataTypeTranslator
    {
        public static string MapToName(ColumnDataType columnDataType)
        {
            return columnDataType.ToString().ToLowerInvariant();
        }
    }
}
