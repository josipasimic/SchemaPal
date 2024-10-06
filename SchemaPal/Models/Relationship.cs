namespace SchemaPal.Models
{
    public class Relationship
    {
        public int RelationshipId { get; set; }

        public int SourceTableId { get; set; }

        public int SourceColumnId { get; set; }

        public int DestinationTableId { get; set; }

        public int DestinationColumnId { get; set; }

        public double X1 { get; set; }

        public double Y1 { get; set; }

        public double X2 { get; set; }

        public double Y2 { get; set; }
    }
}
