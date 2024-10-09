﻿namespace SchemaPal.DataTransferObjects
{
    public class Table
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Column> Columns { get; set; }

        public double CoordinateX { get; set; }

        public string LeftPx
        {
            get { return $"{CoordinateX}px"; }
        }

        public double CoordinateY { get; set; }

        public string TopPx
        {
            get { return $"{CoordinateY}px"; }
        }

        public Table()
        {
            Columns = new List<Column>();
            Name = string.Empty;
        }
    }
}
