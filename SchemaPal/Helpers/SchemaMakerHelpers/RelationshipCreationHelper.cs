using SchemaPal.DataTransferObjects;

namespace SchemaPal.Helpers.SchemaMakerHelpers
{
    public class RelationshipCreationHelper
    {
        public Relationship? CurrentRelationship;

        public bool IsDrawingLine = false;

        public int SourceTableId { get; set; }

        public int DestinationTableId { get; set; }

        public string StartingConnectionPointId { get; set; }

        public RelationshipCreationHelper()
        {
            StartingConnectionPointId = string.Empty;
        }

        public void Reset()
        {
            CurrentRelationship = null;
            IsDrawingLine = false;
            SourceTableId = 0;
            DestinationTableId = 0;
            StartingConnectionPointId = string.Empty;
        }
    }
}
