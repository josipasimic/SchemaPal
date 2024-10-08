using SchemaPal.DataTransferObjects;
using SchemaPal.DataTransferObjects.Enums;
using SchemaPal.Pages.SchemaMakerEnums;
using SchemaPal.Pages.SchemaMakerHelpers;

namespace SchemaPal.Services
{
    public class SchemaObjectFactory : ISchemaObjectFactory
    {
        private static int _tableId = 1;

        private static int _columnId = 1;

        private static int _relationshipId = 1;

        private const string TableNameFormat = "Table @tableId@";

        private readonly ICoordinatesCalculator _coordinatesCalculator;

        public SchemaObjectFactory(ICoordinatesCalculator coordinatesCalculator)
        {
            _coordinatesCalculator = coordinatesCalculator;
        }

        public void CreateNewTable(DatabaseSchema databaseSchema)
        {
            if (databaseSchema is null)
            {
                return;
            }

            var newTable = new Table
            {
                Id = _tableId,
                Name = TableNameFormat.Replace("@tableId@", _tableId.ToString()),
                Columns = new List<Column> { new Column(_columnId, "Id") },
                CoordinateX = SchemaMakerConstants.TableStartingCoordinateX,
                CoordinateY = SchemaMakerConstants.TableStartingCoordinateY
            };

            databaseSchema.Tables.Add(newTable);

            var newColumnPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(_tableId, _columnId, TableSide.Left),
                new ConnectionPoint(_tableId, _columnId, TableSide.Right)
            };

            var connectionPointsXCoordinates = _coordinatesCalculator.CalculateConnectionPointsX(newColumnPoints);
            newColumnPoints.ForEach(cp => cp.ConnectionPointLeftCoordinate = connectionPointsXCoordinates.GetValueOrDefault(cp.UniqueIdentifier));

            var connectionPointsYCoordinate = _coordinatesCalculator.CalculateConnectionPointY(_columnId, newTable);
            newColumnPoints.ForEach(cp => cp.ConnectionPointTopCoordinate = connectionPointsYCoordinate);

            databaseSchema.ConnectionPoints.AddRange(newColumnPoints);

            newColumnPoints.ForEach(
                cp => databaseSchema.ConnectionPointColors[cp.UniqueIdentifier] = SchemaMakerConstants.DefaultConnectionPointColor);

            _tableId++;
            _columnId++;
        }

        public Relationship CreateNewRelationship(ConnectionPoint startingConnectionPoint, 
            Dictionary<string, string> connectionPointColors)
        {
            if (startingConnectionPoint is null)
            {
                return new Relationship();
            }

            var newRelationship = new Relationship
            {
                Id = _relationshipId,
                SourceTableId = startingConnectionPoint.TableId,
                SourceColumnId = startingConnectionPoint.ColumnId,
                RelationshipType = RelationshipType.OneToMany
            };

            connectionPointColors[startingConnectionPoint.UniqueIdentifier] = SchemaMakerConstants.SelectedConnectionPointColor;

            return newRelationship;
        }

        public void CloseNewRelationship(DatabaseSchema databaseSchema, 
            Relationship newRelationship, 
            ConnectionPoint endingConnectionPoint,
            string startingConnectionPointId)
        {
            if (databaseSchema is null
                || newRelationship is null
                || endingConnectionPoint is null)
            {
                return;
            }

            // Ako se spajaju atributi iz iste tablice, prekidamo proces.
            var areConnectionPointTablesEqual = newRelationship.SourceTableId == endingConnectionPoint.TableId;

            var connectingTableIds = new HashSet<int> { newRelationship.SourceTableId, endingConnectionPoint.TableId };
            var connectingColumnIds = new HashSet<int> { newRelationship.SourceColumnId, endingConnectionPoint.ColumnId };

            // Ako veza između atributa već postoji, prekidamo proces.
            var doesConnectionAlreadyExist = databaseSchema.Relationships
                .Any(x => connectingTableIds.Contains(x.SourceTableId)
                    && connectingTableIds.Contains(x.DestinationTableId)
                    && connectingColumnIds.Contains(x.SourceColumnId)
                    && connectingColumnIds.Contains(x.DestinationColumnId));

            if (areConnectionPointTablesEqual
                || doesConnectionAlreadyExist)
            {
                var isStartingPointConnected = databaseSchema.Relationships
                    .Any(r => r.ConnectionPointIds.Start == startingConnectionPointId
                        || r.ConnectionPointIds.End == startingConnectionPointId);
                databaseSchema.ConnectionPointColors[startingConnectionPointId] = isStartingPointConnected
                    ? SchemaMakerConstants.ConnectedConnectionPointColor
                    : SchemaMakerConstants.DefaultConnectionPointColor;

                return;
            }

            newRelationship.DestinationTableId = endingConnectionPoint.TableId;
            newRelationship.DestinationColumnId = endingConnectionPoint.ColumnId;

            newRelationship.ConnectionPointIds = (startingConnectionPointId, endingConnectionPoint.UniqueIdentifier);

            databaseSchema.ConnectionPointColors[endingConnectionPoint.UniqueIdentifier] = SchemaMakerConstants.ConnectedConnectionPointColor;
            databaseSchema.ConnectionPointColors[startingConnectionPointId] = SchemaMakerConstants.ConnectedConnectionPointColor;

            databaseSchema.Relationships.Add(newRelationship);

            _relationshipId++;
        }
    }
}
