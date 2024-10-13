namespace SchemaPal.Enums.EnumTranslators
{
    public static class RelationshipTypeTranslator
    {
        public static string MapToName(RelationshipType relationshipType)
        {
            switch (relationshipType)
            {
                case RelationshipType.OneToOne:
                    return "1:1";
                case RelationshipType.OneToMany:
                    return "1:N";
                case RelationshipType.ManyToOne:
                    return "N:1";
                default:
                    return string.Empty;
            }
        }
    }
}
