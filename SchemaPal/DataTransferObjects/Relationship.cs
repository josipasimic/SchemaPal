using SchemaPal.DataTransferObjects.Enums;
using SchemaPal.Pages.SchemaMakerEnums;

namespace SchemaPal.DataTransferObjects
{
    public class Relationship
    {
        public int Id { get; set; }

        public int SourceTableId { get; set; }

        public int SourceColumnId { get; set; }

        public int DestinationTableId { get; set; }

        public int DestinationColumnId { get; set; }

        public RelationshipType RelationshipType { get; set; }

        public (string Start, string End) ConnectionPointIds { get; set; }

        #region line coordinates 

        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double MidX1 { get; set; }

        public double MidY1 { get; set; }

        public double MidX2 { get; set; }

        public double MidY2 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }

        #endregion
    }
}
