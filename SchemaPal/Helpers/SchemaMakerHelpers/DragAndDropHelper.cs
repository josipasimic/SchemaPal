namespace SchemaPal.Helpers.SchemaMakerHelpers
{
    public class DragAndDropHelper
    {
        public double StartingClientX { get; set; }

        public double StartingClientY { get; set; }

        public int TableId { get; set; }

        public void Reset()
        {
            StartingClientX = 0;
            StartingClientY = 0;
            TableId = 0;
        }
    }
}
