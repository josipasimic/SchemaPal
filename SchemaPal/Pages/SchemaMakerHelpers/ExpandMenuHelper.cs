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
    }
}
