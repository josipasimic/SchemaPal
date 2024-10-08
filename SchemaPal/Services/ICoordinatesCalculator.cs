using SchemaPal.DataTransferObjects;
using SchemaPal.Pages.SchemaMakerEnums;

namespace SchemaPal.Services
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
    }
}
