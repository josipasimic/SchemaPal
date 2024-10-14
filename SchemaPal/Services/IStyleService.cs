using SchemaPal.DataTransferObjects;
using SchemaPal.Enums;

namespace SchemaPal.Services
{
    public interface IStyleService
    {
        double Zoom(double zoomLevel,
            ZoomDirection zoomDirection);

        void SetConnectionPointsColor(
            DatabaseSchema databaseSchema,
            ConnectionPointColorEvent connectionPointColorEvent,
            List<string> connectionPointIds = null,
            HashSet<int> tableIds = null);
    }
}
