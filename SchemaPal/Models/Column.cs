namespace SchemaPal.Models
{
    public class Column
    {
        public int Id { get; set; }

        public int TableId { get; set; }

        public string Name { get; set; }

        public Column(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
