using SchemaPal.Enums;
using SchemaPal.SchemaObjects;

namespace SchemaPal.Services.SchemaMakerServices
{
    public interface ICoordinatesCalculator
    {
        Dictionary<string, double> CalculateConnectionPointsX(List<ConnectionPoint> connectionPoints);

        double CalculateConnectionPointY(int columnId, Table table);

        double CalculateEdgePointX(Table table, TableSide side);

        double CalculateMidPointX(
            (double relationshipStartX, double relationshipEndX, int relationshipsBetweenTablesCount, int indexOfRelationship) relationshipData,
            TableSide overlapSide);

        double CalculateEdgePointY(Table table, List<ConnectionPoint> schemaConnectionPoints, int columnId);

        (double NewX, double NewY) CalculateTableCoordinates(
            (double OldX, double OldY) oldCoordinates,
            (double StartingX, double StartingY) startingPosition,
            (double TargetX, double TargetY) targetPosition,
            double zoomLevel);
    }
}
