namespace SchemaPal.Enums.EnumTranslators
{
    public static class SchemaTemplatesTranslator
    {
        public static string MapToName(SchemaTemplates schemaTemplates)
        {
            switch (schemaTemplates)
            {
                case SchemaTemplates.MusicStreamingApp:
                    return "Glazbena platforma";
                default:
                    return string.Empty;
            }
        }
    }
}
