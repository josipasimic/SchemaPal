using SchemaPal.DataTransferObjects.Enums;

namespace SchemaPal.Pages.SchemaMakerHelpers.EnumTranslators
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
                default:
                    return string.Empty;
            }
        }
    }
}
