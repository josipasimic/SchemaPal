using SchemaPal.Enums;
using SchemaPal.SchemaObjects;

namespace SchemaPal.Helpers.SchemaMakerHelpers
{
    public class RelationshipCreationHelper
    {
        public Relationship? CurrentRelationship;

        public RelationshipCreationMode CreationMode { get; set; }

        public int SourceTableId { get; set; }

        public int DestinationTableId { get; set; }

        public string StartingConnectionPointId { get; set; }

        public string EndingConnectionPointId { get; set; }

        public RelationshipCreationHelper()
        {
            StartingConnectionPointId = string.Empty;
            EndingConnectionPointId = string.Empty;
        }

        public void Reset()
        {
            CurrentRelationship = null;
            SourceTableId = 0;
            DestinationTableId = 0;
            StartingConnectionPointId = string.Empty;
            EndingConnectionPointId = string.Empty;
            CreationMode = RelationshipCreationMode.None;
        }
    }
}
