using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services
{
    public interface IPositionService
    {
        void UpdateTableCoordinates(
            DatabaseSchema databaseSchema,
            int tableId,
            (double StartingX, double StartingY) startingPosition,
            (double TargetX, double TargetY) targetPosition);

        void UpdateRelationshipPositions(
            DatabaseSchema databaseSchema,
            HashSet<int> tableIds = null);
    }
}
