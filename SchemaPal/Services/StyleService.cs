using SchemaPal.DataTransferObjects;
using SchemaPal.Enums;
using SchemaPal.Helpers.SchemaMakerHelpers;

namespace SchemaPal.Services
{
    public class StyleService : IStyleService
    {
        public double Zoom(double zoomLevel, 
            ZoomDirection zoomDirection)
        {
            var stepSign = zoomDirection == ZoomDirection.In ? 1 : -1;

            return Math.Clamp(
                zoomLevel + stepSign * SchemaMakerConstants.ZoomLevelStep,
                SchemaMakerConstants.MinimalZoomLevel,
                SchemaMakerConstants.MaximalZoomLevel);
        }

        public void SetConnectionPointsColor(
            DatabaseSchema databaseSchema, 
            ConnectionPointColorEvent connectionPointColorEvent,
            List<string> connectionPointIds = null, 
            HashSet<int> tableIds = null)
        {
            if (databaseSchema is null
                || connectionPointColorEvent.Equals(ConnectionPointColorEvent.None))
            {
                return;
            }

            if (connectionPointIds is null
                || connectionPointIds.Count == 0)
            {
                connectionPointIds = databaseSchema.ConnectionPoints
                    .Select(x => x.UniqueIdentifier)
                    .ToList();
            }

            if (tableIds != null
                && tableIds.Count > 0)
            {
                connectionPointIds = databaseSchema.ConnectionPoints
                    .Where(cp => connectionPointIds.Contains(cp.UniqueIdentifier)
                        && tableIds.Contains(cp.TableId))
                    .Select(x => x.UniqueIdentifier)
                    .ToList();
            }

            switch (connectionPointColorEvent)
            {
                case ConnectionPointColorEvent.Create:
                    connectionPointIds.ForEach(cp => databaseSchema.ConnectionPointColors[cp] = SchemaMakerConstants.DefaultConnectionPointColor);
                    break;
                case ConnectionPointColorEvent.Select:
                    connectionPointIds.ForEach(cp => databaseSchema.ConnectionPointColors[cp] = SchemaMakerConstants.SelectedConnectionPointColor);
                    break;
                case ConnectionPointColorEvent.Reset:
                    ResetConnectionPointColors(connectionPointIds, databaseSchema);
                    break;
                default:
                    break;
            }
        }

        private void ResetConnectionPointColors(List<string> connectionPointIds,
            DatabaseSchema databaseSchema)
        {
            if (connectionPointIds is null
                || connectionPointIds.Count == 0)
            {
                return;
            }

            var connectedConnectionPointIds = databaseSchema.Relationships
                .SelectMany(r => new[] { r.ConnectionPointIds.Start, r.ConnectionPointIds.End })
                .Where(cp => connectionPointIds.Contains(cp))
                .Distinct()
                .ToList();

            connectedConnectionPointIds.ForEach(cp => databaseSchema.ConnectionPointColors[cp] = SchemaMakerConstants.ConnectedConnectionPointColor);

            var unconnectedConnectionPoints = connectionPointIds
                .Except(connectedConnectionPointIds)
                .Distinct()
                .ToList();

            unconnectedConnectionPoints.ForEach(cp => databaseSchema.ConnectionPointColors[cp] = SchemaMakerConstants.DefaultConnectionPointColor);
        }
    }
}
