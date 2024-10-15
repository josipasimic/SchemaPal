using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services.SchemaMakerServices
{
    public interface IPositionService
    {
        void UpdateTablePosition(
            Table table,
            (double StartingX, double StartingY) startingPosition,
            (double TargetX, double TargetY) targetPosition,
            double zoomLevel);

        void UpdateRelationshipPositions(
            DatabaseSchema databaseSchema,
            HashSet<int> tableIds = null);
    }
}
