namespace SchemaPal.DataTransferObjects
{
    public class DatabaseSchema
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Table> Tables { get; set; }

        public List<Relationship> Relationships { get; set; }

        public List<ConnectionPoint>  ConnectionPoints { get; set; }

        public Dictionary<string, string> ConnectionPointColors { get; set; }

        public DatabaseSchema()
        {
            Tables = new List<Table>();
            Relationships = new List<Relationship>();
            ConnectionPoints = new List<ConnectionPoint>();
            ConnectionPointColors = new Dictionary<string, string>();
            Name = string.Empty;
        }
    }
}
