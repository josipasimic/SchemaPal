using SchemaPal.DataTransferObjects;
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

        public int CreateNewTable(DatabaseSchema databaseSchema)
        {
            if (databaseSchema is null)
            {
                return 0;
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

            _tableId++;
            _columnId++;

            return newTable.Id;
        }

        public Relationship CreateNewRelationship(ConnectionPoint startingConnectionPoint)
        {
            if (startingConnectionPoint is null)
            {
                return new Relationship();
            }

            var newRelationship = new Relationship
            {
                Id = _relationshipId,
                SourceTableId = startingConnectionPoint.TableId,
                SourceColumnId = startingConnectionPoint.ColumnId
            };

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
            if (areConnectionPointTablesEqual)
            {
                return;
            }

            var connectingTableIds = new HashSet<int> { newRelationship.SourceTableId, endingConnectionPoint.TableId };
            var connectingColumnIds = new HashSet<int> { newRelationship.SourceColumnId, endingConnectionPoint.ColumnId };

            // Ako veza između atributa već postoji, prekidamo proces.
            var doesConnectionAlreadyExist = databaseSchema.Relationships
                .Any(x => connectingTableIds.Contains(x.SourceTableId)
                    && connectingTableIds.Contains(x.DestinationTableId)
                    && connectingColumnIds.Contains(x.SourceColumnId)
                    && connectingColumnIds.Contains(x.DestinationColumnId));
            if (doesConnectionAlreadyExist)
            {
                return;
            }

            newRelationship.DestinationTableId = endingConnectionPoint.TableId;
            newRelationship.DestinationColumnId = endingConnectionPoint.ColumnId;

            newRelationship.ConnectionPointIds = (startingConnectionPointId, endingConnectionPoint.UniqueIdentifier);

            databaseSchema.Relationships.Add(newRelationship);

            _relationshipId++;
        }
    }
}
