using SchemaPal.Models;

namespace SchemaPal.Pages.SchemaMakerHelpers
{
    public class RelationshipHelper
    {
        public Relationship? CurrentRelationship;

        public bool IsDrawingLine = false;

        public int SourceTableId { get; set; }

        public int DestinationTableId { get; set; }

        public void Reset()
        {
            CurrentRelationship = null;
            IsDrawingLine = false;
            SourceTableId = 0;
            DestinationTableId = 0;
        }
    }
}
