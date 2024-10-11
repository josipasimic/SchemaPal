using Microsoft.Extensions.Primitives;

namespace SchemaPal.Pages.SchemaMakerHelpers
{
    public static class SchemaMakerConstants
    {
        public const double TableWidth = 200;

        public static string TableWidthPx
        {
            get { return $"{TableWidth}px"; }
        }

        public const double TablePadding = 10;

        public static string TablePaddingPx
        {
            get { return $"{TablePadding}px"; }
        }

        public const double TableFontSize = 20;

        public static string TableFontSizePx
        {
            get { return $"{TableFontSize}px"; }
        }

        public const double ColumnPadding = 5;

        public static string ColumnPaddingPx
        {
            get { return $"{ColumnPadding}px"; }
        }

        public const double ColumnTopMargin = 5;

        public static string ColumnTopMarginPx
        {
            get { return $"{ColumnTopMargin}px"; }
        }

        public const double ColumnFontSize = 17;

        public static string ColumnFontSizePx
        {
            get { return $"{ColumnFontSize}px"; }
        }

        public const double TableStartingCoordinateX = 50;

        public const double TableStartingCoordinateY = 100;

        public const string DefaultConnectionPointColor = "#bbb";

        public const string ConnectedConnectionPointColor = "rgb(133, 6, 6, 0.7)";

        public const string SelectedConnectionPointColor = "red";

        public const double MinimalZoomLevel = 0.5;

        public const double MaximalZoomLevel = 3.0;

        public const double ZoomLevelStep = 0.1;

        public const double StraightLineRelationshipTreshold = 70;

        public const double OverlapMidpointStepForRelationshipLine = 20;

        public const double ColumnFontSizeBuffer = 10;

        public const double ConnectionPointBuffer = 2.5;

        public const double SnapshotContentMargin = 20;
    }
}
