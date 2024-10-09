using SchemaPal.DataTransferObjects;
using SchemaPal.Pages.SchemaMakerEnums;
using SchemaPal.Pages.SchemaMakerHelpers;

namespace SchemaPal.Services
{
    public class CoordinatesCalculator : ICoordinatesCalculator
    {
        public Dictionary<string, double> CalculateConnectionPointsX(List<ConnectionPoint> connectionPoints)
        {
            var connectionPointCoordinatesX = new Dictionary<string, double>();

            if (connectionPoints is null)
            {
                return connectionPointCoordinatesX;
            }

            foreach (var connectionPoint in connectionPoints)
            {
                switch (connectionPoint.Side)
                {
                    case TableSide.Left:
                        connectionPointCoordinatesX[connectionPoint.UniqueIdentifier] = 0;
                        break;
                    case TableSide.Right:
                        connectionPointCoordinatesX[connectionPoint.UniqueIdentifier] = SchemaMakerConstants.TableWidth - SchemaMakerConstants.TablePadding;
                        break;
                    case TableSide.None:
                    default:
                        break;
                }
            }

            return connectionPointCoordinatesX;
        }

        public double CalculateConnectionPointY(int columnId, Table table)
        {
            if (table is null)
            {
                return 0;
            }

            var columnIndex = table.Columns.IndexOf(table.Columns.First(c => c.Id == columnId));

            var columnHeight = SchemaMakerConstants.ColumnFontSize
                + 2 * SchemaMakerConstants.ColumnPadding;

            var spaceBetweenPoints = columnHeight
                + SchemaMakerConstants.ColumnTopMargin
                + SchemaMakerConstants.ColumnFontSize;

            var additionalStartingLength = SchemaMakerConstants.TableFontSize
                + SchemaMakerConstants.TablePadding
                + SchemaMakerConstants.ColumnTopMargin;

            return columnIndex * spaceBetweenPoints + additionalStartingLength + columnHeight;
        }

        public double CalculateEdgePointX(Table table, TableSide side)
        {
            switch (side)
            {
                case TableSide.Left:
                    return table.CoordinateX;
                case TableSide.Right:
                    return table.CoordinateX + SchemaMakerConstants.TableWidth;
                case TableSide.None:
                default:
                    return 0;
            }
        }

        public double CalculateMidPointX(
            (double relationshipStartX, double relationshipEndX, int relationshipsBetweenTablesCount, int indexOfRelationship) relationshipData,
            TableSide overlapSide)
        {
            var x1 = relationshipData.relationshipStartX;
            var x2 = relationshipData.relationshipEndX;

            var startingPoint = x1;
            var distanceBetweenPointsX = Math.Abs(x1 - x2);
            var stepMultiplier = relationshipData.indexOfRelationship + 1;

            var pointShiftX = 0.0;
            switch (overlapSide)
            {
                // Ako se tablice vertikalno preklapaju, želimo da linije idu "okolo njih", pa
                // x-koordinate moramo pomaknuti ulijevo/udesno ovisno o tome koja je strana preklapanja.
                case TableSide.Left:
                    var complementaryLeftDistance = Math.Abs(x2 - (x1 + SchemaMakerConstants.TableWidth));
                    pointShiftX = -1 * (Math.Min(distanceBetweenPointsX, complementaryLeftDistance)
                        + stepMultiplier * SchemaMakerConstants.OverlapMidpointStepForRelationshipLine);
                    break;
                case TableSide.Right:
                    var complementaryRightDistance = Math.Abs(x1 - (x2 + SchemaMakerConstants.TableWidth));
                    pointShiftX = Math.Min(distanceBetweenPointsX, complementaryRightDistance)
                        + stepMultiplier * SchemaMakerConstants.OverlapMidpointStepForRelationshipLine;
                    break;
                // Inače, ako se tablice ne preklapaju vertikalno, i y-koordinate su "dovoljno" daleko da želimo prelomiti liniju, udaljenost od početne
                // i završne točke x-koordinate dijelimo na ekvidistantne dijelove ovisno o broju veza između tablica, kako bismo dobili pomak za svaku vezu
                // između tablica. Tada, točki prelamanja pridodijelimo odgovarajući x-pomak ovisno o tome je li trenutna veza bliže početku ili kraju.
                case TableSide.None:
                    startingPoint = Math.Min(x1, x2);
                    stepMultiplier = startingPoint == x2 
                        ? relationshipData.relationshipsBetweenTablesCount - relationshipData.indexOfRelationship 
                        : stepMultiplier;
                    pointShiftX = stepMultiplier * distanceBetweenPointsX / (relationshipData.relationshipsBetweenTablesCount + 1);
                    break;
                default:
                    break;
            }

            return startingPoint + pointShiftX;
        }

        public double CalculateEdgePointY(Table table, List<ConnectionPoint> schemaConnectionPoints, int columnId)
        {
            if (table is null
                || schemaConnectionPoints is null)
            {
                return 0;
            }

            var column = table.Columns.FirstOrDefault(x => x.Id == columnId);

            if (column is null)
            {
                return 0;
            }

            var columnIndex = table.Columns.IndexOf(column);
            var connectionPointForColumn = schemaConnectionPoints.FirstOrDefault(cp => cp.TableId == table.Id
                && cp.ColumnId == columnId);

            if (connectionPointForColumn is null)
            {
                return 0;
            }

            return table.CoordinateY + connectionPointForColumn.ConnectionPointTopCoordinate + SchemaMakerConstants.ConnectionPointBuffer;
        }
    }
}
