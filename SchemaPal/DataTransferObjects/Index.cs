using System.Text;
using SchemaPal.Enums;
using SchemaPal.Enums.EnumTranslators;

namespace SchemaPal.DataTransferObjects
{
    public class Index
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TableId { get; set; }

        public List<(int Id, string Name)> Columns { get; set; }

        public IndexType Type { get; set; }

        public KeyType KeyType { get; set; }

        public string FullIndexName
        {
            get
            {
                var typeAbbreviations = new StringBuilder();

                if (KeyType != KeyType.None)
                {
                    var keyTypeAbbreviation = KeyTypeTranslator.GetAbbreviation(KeyType);
                    typeAbbreviations.Append($"{keyTypeAbbreviation} ");
                }

                typeAbbreviations.Append(IndexTypeTranslator.GetAbbreviation(Type));

                var columnNames = string.Join(", ", Columns.Select(c => c.Name));

                return $"{typeAbbreviations} {Name} ({columnNames})";
            }
        }

        public Index(int id, string name, int tableId)
        {
            Id = id;
            Name = name;
            TableId = tableId;
            Columns = new List<(int Id, string Name)>();
        }
    }
}
