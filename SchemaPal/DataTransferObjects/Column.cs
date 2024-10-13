using System.Text;
using SchemaPal.Enums;
using SchemaPal.Enums.EnumTranslators;

namespace SchemaPal.DataTransferObjects
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
                var fullName = new StringBuilder();

                var dataType = ColumnDataTypeTranslator.MapToName(DataType);
                fullName.Append($"{dataType} (");

                if (KeyType != KeyType.None)
                {
                    fullName.Append($"{KeyTypeTranslator.GetAbbreviation(KeyType)}, ");
                }

                var nullableAbbreviation = IsNullable ? "NULL" : "NOT NULL";
                fullName.Append($"{nullableAbbreviation})");

                return fullName.ToString();
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
