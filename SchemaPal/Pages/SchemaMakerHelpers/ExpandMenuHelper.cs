namespace SchemaPal.Pages.SchemaMakerHelpers
{
    public class ExpandMenuHelper
    {
        public bool AreTablesExpanded { get; set; }

        public bool AreRelationshipsExpanded { get; set; }

        public HashSet<int> ExpandedTableIds { get; set; }

        public HashSet<int> ExpandedColumnsTableIds { get; set; }

        public HashSet<int> ExpandedIndexTableIds { get; set; }

        public HashSet<int> ExpandedColumnIds { get; set; }

        public HashSet<int> ExpandedIndexIds { get; set; }

        public ExpandMenuHelper() 
        {
            ExpandedTableIds = new HashSet<int>();
            ExpandedColumnsTableIds = new HashSet<int>();
            ExpandedIndexTableIds = new HashSet<int>();
            ExpandedColumnIds = new HashSet<int>();
            ExpandedIndexIds = new HashSet<int>();
        }

        public void ToggleTablesExpanded() => AreTablesExpanded = !AreTablesExpanded;

        public void ToggleRelationshipsExpanded() => AreRelationshipsExpanded = !AreRelationshipsExpanded;

        public void ToggleTableDetails(int tableId)
        {
            if (ExpandedTableIds.Contains(tableId))
                ExpandedTableIds.Remove(tableId);
            else
                ExpandedTableIds.Add(tableId);
        }

        public void ToggleColumnsExpanded(int tableId)
        {
            if (ExpandedColumnsTableIds.Contains(tableId))
                ExpandedColumnsTableIds.Remove(tableId);
            else
                ExpandedColumnsTableIds.Add(tableId);
        }

        public void ToggleIndexExpanded(int tableId)
        {
            if (ExpandedIndexTableIds.Contains(tableId))
                ExpandedIndexTableIds.Remove(tableId);
            else
                ExpandedIndexTableIds.Add(tableId);
        }

        public void ToggleColumnDetails(int columnId)
        {
            if (ExpandedColumnIds.Contains(columnId))
                ExpandedColumnIds.Remove(columnId);
            else
                ExpandedColumnIds.Add(columnId);
        }

        public void ToggleIndexDetails(int indexId)
        {
            if (ExpandedIndexIds.Contains(indexId))
                ExpandedIndexIds.Remove(indexId);
            else
                ExpandedIndexIds.Add(indexId);
        }
    }
}
