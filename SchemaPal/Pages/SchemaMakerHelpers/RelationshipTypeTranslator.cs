using SchemaPal.DataTransferObjects.Enums;

namespace SchemaPal.Pages.SchemaMakerHelpers
{
    public static class RelationshipTypeTranslator
    {
        public static string MapToTagText(RelationshipType relationshipType)
        {
            switch (relationshipType)
            {
                case RelationshipType.OneToOne:
                    return "1:1";
                case RelationshipType.OneToMany:
                    return "1:N";
                case RelationshipType.ManyToOne:
                    return "N:1";
                case RelationshipType.ManyToMany:
                    return "N:N";
                case RelationshipType.None:
                default:
                    return string.Empty;
            }
        }
    }
}
