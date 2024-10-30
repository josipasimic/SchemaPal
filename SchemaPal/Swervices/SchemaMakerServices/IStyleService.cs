using SchemaPal.Enums;
using SchemaPal.SchemaElements;

namespace SchemaPal.Services.SchemaMakerServices
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
