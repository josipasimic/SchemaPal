using SchemaPal.Enums;

namespace SchemaPal.SchemaElements
{
    public class Relationship : SidebarExpandableElement
    {
        public int Id { get; set; }

        public int SourceTableId { get; set; }

        public int SourceColumnId { get; set; }

        public int DestinationTableId { get; set; }

        public int DestinationColumnId { get; set; }

        public RelationshipType RelationshipType { get; set; }

        public RelationshipConnectionPointIds ConnectionPointIds { get; set; } = new RelationshipConnectionPointIds();

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

    public class RelationshipConnectionPointIds
    {
        public string Start { get; set;}

        public string End { get; set;}
    }
}
