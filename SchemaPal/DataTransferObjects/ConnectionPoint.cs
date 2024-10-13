using SchemaPal.Enums;

namespace SchemaPal.DataTransferObjects
{
    public class ConnectionPoint
    {
        public int TableId { get; set; }

        public int ColumnId { get; set; }

        public TableSide Side { get; set; }

        public double ConnectionPointLeftCoordinate { get; set; }

        public string ConnectionPointLeftCoordinatePx
        {
            get
            {
                return $"{ConnectionPointLeftCoordinate}px";
            }
        }

        public double ConnectionPointTopCoordinate { get; set; }

        public string ConnectionPointTopCoordinatePx
        {
            get
            {
                return $"{ConnectionPointTopCoordinate}px";
            }
        }

        public string UniqueIdentifier
        {
            get { return $"{TableId}_{ColumnId}_{(int)Side}"; }
        }

        public ConnectionPoint(int tableId, int columnId, TableSide side)
        {
            TableId = tableId;
            ColumnId = columnId;
            Side = side;
        }
    }
}
