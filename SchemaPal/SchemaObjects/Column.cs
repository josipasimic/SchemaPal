using System.Text;
using SchemaPal.Enums;
using SchemaPal.Enums.EnumTranslators;

namespace SchemaPal.SchemaObjects
{
    public class Column
    {
        public int Id { get; set; }

        public int TableId { get; set; }

        public string Name { get; set; }

        public ColumnDataType DataType { get; set; }

        public KeyType KeyType { get; set; }

        public bool IsNullable { get; set; }

        public string ColumnProperties
        {
            get
            {
                var properties = new StringBuilder();

                var dataType = ColumnDataTypeTranslator.MapToName(DataType);
                properties.Append($"{dataType}");

                if (IsNullable)
                {
                    properties.Append('?');
                }

                if (KeyType != KeyType.None)
                {
                    properties.Append($" ({KeyTypeTranslator.GetAbbreviation(KeyType)})");
                }

                return properties.ToString();
            }
        }

        public Column(int id, string name, int tableId)
        {
            Id = id;
            Name = name;
            TableId = tableId;
        }
    }
}
