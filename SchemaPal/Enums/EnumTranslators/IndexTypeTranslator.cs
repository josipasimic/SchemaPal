namespace SchemaPal.Enums.EnumTranslators
{
    public static class IndexTypeTranslator
    {
        public static string MapToName(IndexType indexType)
        {
            switch (indexType)
            {
                case IndexType.NonClustered:
                    return "Neklasterirani";
                case IndexType.Clustered:
                    return "Klasterirani";
                case IndexType.Hash:
                    return "Hash";
                case IndexType.Prostorni:
                    return "Prostorni";
                case IndexType.XML:
                    return "XML";
                case IndexType.FullText:
                    return "Full-text";
                default:
                    return string.Empty;
            }
        }

        public static string GetAbbreviation(IndexType indexType)
        {
            switch (indexType)
            {
                case IndexType.NonClustered:
                    return "NCL IX";
                case IndexType.Clustered:
                    return "CL IX";
                case IndexType.Hash:
                    return "HASH IX";
                case IndexType.Prostorni:
                    return "SP IX";
                case IndexType.XML:
                    return "XML IX";
                case IndexType.FullText:
                    return "FT IX";
                default:
                    return string.Empty;
            }
        }
    }
}
