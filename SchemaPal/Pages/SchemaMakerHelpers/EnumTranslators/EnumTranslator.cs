namespace SchemaPal.Pages.SchemaMakerHelpers.EnumTranslators
{
    public static class EnumTranslator
    {
        public static List<string> GetNamesList<TEnum>(
            Func<TEnum, string> mapToName, 
            params TEnum[] excludeValues) where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            if (excludeValues != null)
            {
                values = values.Except(excludeValues);
            }

            var names = values
                .Select(mapToName)
                .ToList();

            return names;
        }
    }
}
