using SchemaPal.Enums;

namespace SchemaPal.SchemaElements
{
    public class Index : SidebarExpandableElement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TableId { get; set; }

        public List<int> ColumnIds { get; set; }

        public IndexType Type { get; set; }

        public KeyType KeyType { get; set; }

        public Index(int id, string name, int tableId)
        {
            Id = id;
            Name = name;
            TableId = tableId;
            ColumnIds = new List<int>();
        }
    }
}
