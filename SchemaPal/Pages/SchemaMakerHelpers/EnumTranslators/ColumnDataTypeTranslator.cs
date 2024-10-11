using SchemaPal.DataTransferObjects.Enums;

namespace SchemaPal.Pages.SchemaMakerHelpers.EnumTranslators
{
    public static class ColumnDataTypeTranslator
    {
        public static string MapToName(ColumnDataType columnDataType)
        {

            return columnDataType.ToString().ToLowerInvariant();
        }

        public static List<string> GetNamesList()
        {
            return EnumTranslator.GetNamesList<ColumnDataType>(MapToName);
        }
    }
}
