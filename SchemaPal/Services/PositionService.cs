using SchemaPal.DataTransferObjects;
using SchemaPal.Enums;
using SchemaPal.Helpers.SchemaMakerHelpers;

namespace SchemaPal.Services
{
    public class PositionService : IPositionService
    {
        private readonly ICoordinatesCalculator _coordinatesCalculator;

        public PositionService(ICoordinatesCalculator coordinatesCalculator) 
        {
            _coordinatesCalculator = coordinatesCalculator;
        }

        public void UpdateTablePosition(
            Table table,
            (double StartingX, double StartingY) startingPosition,
            (double TargetX, double TargetY) targetPosition,
            double zoomLevel)
        {
            if (table is null)
            {
                return;
            }

            var newCoordinates = _coordinatesCalculator.CalculateTableCoordinates(
                (table.CoordinateX, table.CoordinateY),
                startingPosition,
                targetPosition,
                zoomLevel);

            table.CoordinateX = newCoordinates.NewX;
            table.CoordinateY = newCoordinates.NewY;
        }

        public void UpdateRelationshipPositions(
            DatabaseSchema databaseSchema,
            HashSet<int> tableIds = null)
        {
            if (databaseSchema is null
                || databaseSchema.Relationships is null)
            {
                return;
            }

            var relationshipsToUpdate = databaseSchema.Relationships;
            if (tableIds != null
                && tableIds.Count > 0)
            {
                relationshipsToUpdate = databaseSchema.Relationships
                    .Where(r => tableIds.Contains(r.SourceTableId) 
                        || tableIds.Contains(r.DestinationTableId))
                    .ToList();
            }

            foreach (var relationship in relationshipsToUpdate)
            {
                var sourceTable = databaseSchema.Tables.First(t => t.Id == relationship.SourceTableId);
                var destinationTable = databaseSchema.Tables.First(t => t.Id == relationship.DestinationTableId);

                var tableSides = DetermineTableSides(sourceTable, destinationTable);

                UpdateRelationshipEdgePoints(relationship,
                    databaseSchema.ConnectionPoints,
                    (sourceTable, tableSides.FirstTableSide),
                    (destinationTable, tableSides.SecondTableSide));

                UpdateRelationshipMidPoints(relationship,
                    databaseSchema.Relationships,
                    tableSides.OverlapSide);
            }
        }

        #region helper methods

        private (TableSide FirstTableSide, TableSide SecondTableSide, TableSide OverlapSide) DetermineTableSides(
            Table firstTable, Table secondTable)
        {
            // Ako se tablice vertikalno preklapaju (jedna su ispod druge vertikalno, tj. "intervali" x-koordinata imaju neprazan
            // presjek), želimo da linije idu "okolo njih", a ne dijagonalno ispod njih, pa ih zato spajamo s iste strane.
            var doTablesVerticallyOverlap = firstTable.CoordinateX <= secondTable.CoordinateX + SchemaMakerConstants.TableWidth
                && secondTable.CoordinateX <= firstTable.CoordinateX + SchemaMakerConstants.TableWidth;

            if (doTablesVerticallyOverlap)
            {
                return firstTable.CoordinateX < secondTable.CoordinateX
                    ? (TableSide.Right, TableSide.Right, TableSide.Right)
                    : (TableSide.Left, TableSide.Left, TableSide.Left);
            }

            // Inače, ako se tablice ne preklapaju vertikalno, želimo da linija ide dijagonalno od jedne tablice do druge.
            if (firstTable.CoordinateX < secondTable.CoordinateX)
            {
                return (TableSide.Right, TableSide.Left, TableSide.None);
            }

            return (TableSide.Left, TableSide.Right, TableSide.None);
        }

        private void UpdateRelationshipEdgePoints(Relationship relationship,
            List<ConnectionPoint> schemaConnectionPoints,
            (Table Table, TableSide Side) sourceTableData,
            (Table Table, TableSide Side) destinationTableData)
        {
            relationship.X1 = _coordinatesCalculator.CalculateEdgePointX(sourceTableData.Table, sourceTableData.Side);
            relationship.Y1 = _coordinatesCalculator.CalculateEdgePointY(sourceTableData.Table, schemaConnectionPoints, relationship.SourceColumnId);

            relationship.X2 = _coordinatesCalculator.CalculateEdgePointX(destinationTableData.Table, destinationTableData.Side);
            relationship.Y2 = _coordinatesCalculator.CalculateEdgePointY(destinationTableData.Table, schemaConnectionPoints, relationship.DestinationColumnId);

            var updatedStartingConnectionPointId = schemaConnectionPoints.First(
                x => x.TableId == sourceTableData.Table.Id
                    && x.ColumnId == relationship.SourceColumnId
                    && x.Side == sourceTableData.Side)
                .UniqueIdentifier;

            var updatedEndingConnectionPointId = schemaConnectionPoints.First(
                x => x.TableId == destinationTableData.Table.Id
                    && x.ColumnId == relationship.DestinationColumnId
                    && x.Side == destinationTableData.Side)
                .UniqueIdentifier;

            relationship.ConnectionPointIds = (updatedStartingConnectionPointId, updatedEndingConnectionPointId);
        }

        private void UpdateRelationshipMidPoints(
            Relationship relationshipToUpdate,
            List<Relationship> allSchemaRelationships,
            TableSide overlapSide)
        {
            if (allSchemaRelationships is null
                || relationshipToUpdate is null)
            {
                return;
            }

            // Ako su y-koordinate tablica dovoljno blizu, i tablice se ne preklapaju vertikalno,
            // crtamo samo ravnu liniju između tablica, ne želimo imati točke prelamanja.
            if (Math.Abs(relationshipToUpdate.Y1 - relationshipToUpdate.Y2) < SchemaMakerConstants.StraightLineRelationshipTreshold
                && overlapSide == TableSide.None)
            {
                relationshipToUpdate.MidX1 = relationshipToUpdate.X1;
                relationshipToUpdate.MidX2 = relationshipToUpdate.X2;
            }
            else
            {
                var tableIds = new List<int> { relationshipToUpdate.SourceTableId, relationshipToUpdate.DestinationTableId };

                var relationshipsBetweenTables = allSchemaRelationships
                    .Where(x => tableIds.Contains(x.SourceTableId)
                        && tableIds.Contains(x.DestinationTableId))
                    .Distinct()
                    .ToList();

                var indexOfRelationshipToUpdate = relationshipsBetweenTables.IndexOf(relationshipToUpdate);

                var midX = _coordinatesCalculator.CalculateMidPointX(
                    (relationshipToUpdate.X1, relationshipToUpdate.X2, relationshipsBetweenTables.Count, indexOfRelationshipToUpdate),
                    overlapSide);

                relationshipToUpdate.MidX1 = relationshipToUpdate.MidX2 = midX;
            }

            relationshipToUpdate.MidY1 = relationshipToUpdate.Y1;
            relationshipToUpdate.MidY2 = relationshipToUpdate.Y2;
        }

        #endregion
    }
}
