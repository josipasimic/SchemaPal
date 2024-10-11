using SchemaPal.DataTransferObjects;
using SchemaPal.Pages.SchemaMakerEnums;
using SchemaPal.Pages.SchemaMakerHelpers;

namespace SchemaPal.Services
{
    public class SchemaObjectFactory : ISchemaObjectFactory
    {
        private static int _tableId = 1;
        private static int _columnId = 1;
        private static int _indexId = 1;
        private static int _relationshipId = 1;

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
                Name = $"Tablica {_tableId}",
                Columns = new List<Column> { CreatePrimaryKeyColumn(_columnId) },
                CoordinateX = SchemaMakerConstants.TableStartingCoordinateX,
                CoordinateY = SchemaMakerConstants.TableStartingCoordinateY
            };
            databaseSchema.Tables.Add(newTable);

            var newColumnPoints = CreateConnectionPointsForColumn(newTable, _columnId);
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

        public void CreateNewColumn(DatabaseSchema databaseSchema, int tableId)
        {
            if (databaseSchema?.Tables is null)
            {
                return;
            }

            var table = databaseSchema.Tables.Find(t => t.Id == tableId);
            if (table is null)
            {
                return;
            }

            var newColumn = (Column)null;
            if (table.Columns.Count == 0) 
            {
                newColumn = CreatePrimaryKeyColumn(_columnId);
            }
            else
            {
                newColumn = new Column(_columnId, $"Stupac {_columnId}")
                {
                    IsNullable = true
                };
            }

            table.Columns.Add(newColumn);

            var newConnectionPoints = CreateConnectionPointsForColumn(table, _columnId);
            databaseSchema.ConnectionPoints.AddRange(newConnectionPoints);

            _columnId++;
        }

        public void CreateNewIndex(Table table)
        {
            if (table is null)
            {
                return;
            }

            var newIndex = new DataTransferObjects.Index(_indexId, string.Empty, table.Id);
            table.Indexes.Add(newIndex);

            _indexId++;
        }

        public void UpdateColumnsOnIndex(Table table, int indexId, List<int> columnIds)
        {
            if (table is null)
            {
                return;
            }

            var columnsToAdd = table.Columns
                .Where(x => columnIds.Contains(x.Id))
                .Select(x => (x.Id, x.Name))
                .ToList();

            var index = table.Indexes.Find(i => i.Id == indexId);
            if (index?.Columns is null)
            {
                return;
            }

            index.Columns.Clear();
            index.Columns.AddRange(columnsToAdd);
        }

        public void DeleteTable(DatabaseSchema databaseSchema, int tableId)
        {
            if (databaseSchema?.Tables is null)
            {
                return;
            }

            databaseSchema.Tables.RemoveAll(t => t.Id == tableId);
        }

        public void DeleteColumn(DatabaseSchema databaseSchema, int tableId, int columnId)
        {
            if (databaseSchema?.Tables is null
                || databaseSchema?.ConnectionPoints is null)
            {
                return;
            }

            var table = databaseSchema.Tables.Find(t => t.Id == tableId);
            if (table?.Columns is null)
            {
                return;
            }

            table.Columns.RemoveAll(c => c.Id == columnId);
            databaseSchema.ConnectionPoints.RemoveAll(c => c.ColumnId == columnId);

            // Ponovno izračunaj visine točke spajanja za preostale stupce u tablici.
            var remainingTableConnectionPoints = databaseSchema.ConnectionPoints
                .Where(cp => cp.TableId == tableId)
                .GroupBy(cp => cp.ColumnId);

            foreach (var connectionPointsForColumn in remainingTableConnectionPoints)
            {
                var connectionPointsYCoordinate = _coordinatesCalculator.CalculateConnectionPointY(
                    connectionPointsForColumn.Key, 
                    table);

                connectionPointsForColumn.ToList()
                    .ForEach(cp => cp.ConnectionPointTopCoordinate = connectionPointsYCoordinate);
            }
        }

        public void DeleteIndexes(DatabaseSchema databaseSchema, 
            int tableId,
            int? indexId = null,
            int? columnId = null)
        {
            if (databaseSchema?.Tables is null)
            {
                return;
            }

            var table = databaseSchema.Tables.Find(t => t.Id == tableId);
            if (table?.Indexes is null)
            {
                return;
            }

            table.Indexes.RemoveAll(i => i.Id == indexId);
            table.Indexes.RemoveAll(i => i.Columns.Any(i => i.Id == columnId));
        }

        public HashSet<int> DeleteRelationships(DatabaseSchema databaseSchema,
            int? relationshipId = null,
            int? tableId = null,
            int? columnId = null)
        {
            if (databaseSchema?.Relationships is null)
            {
                return new HashSet<int>();
            }

            var relationshipIdsToDelete = new List<int>();

            if (relationshipId.HasValue)
            {
                relationshipIdsToDelete.Add(relationshipId.Value);
            }

            if (tableId.HasValue)
            {
                var relationships = databaseSchema.Relationships
                    .Where(r => r.SourceTableId == tableId
                        || r.DestinationTableId == tableId);

                if (columnId.HasValue)
                {
                    relationships = relationships
                        .Where(r => r.SourceColumnId == columnId
                            || r.DestinationColumnId == columnId);
                }

                var relationshipIds = relationships
                    .Select(r => r.Id)
                    .ToList();

                relationshipIdsToDelete.AddRange(relationshipIds); 
            }

            var affectedTableIds = databaseSchema.Relationships
                .Where(r => relationshipIdsToDelete.Contains(r.Id))
                .SelectMany(r => new[] { r.SourceTableId, r.DestinationTableId })
                .Except([tableId ?? -1])
                .ToHashSet();

            databaseSchema.Relationships.RemoveAll(r => relationshipIdsToDelete.Contains(r.Id));

            return affectedTableIds;
        }

        public void Reset()
        {
            _tableId = 1;
            _columnId = 1;
            _indexId = 1;
            _relationshipId = 1;
        }

        #region helper methods

        private Column CreatePrimaryKeyColumn(int columnId)
        {
            return new Column(columnId, "Id")
            {
                KeyType = DataTransferObjects.Enums.KeyType.Primary,
                IsNullable = false
            };
        }

        private List<ConnectionPoint> CreateConnectionPointsForColumn(Table table, int columnId)
        {
            var newColumnPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(table.Id, columnId, TableSide.Left),
                new ConnectionPoint(table.Id, columnId, TableSide.Right)
            };

            var connectionPointsXCoordinates = _coordinatesCalculator.CalculateConnectionPointsX(newColumnPoints);
            newColumnPoints.ForEach(cp => cp.ConnectionPointLeftCoordinate = connectionPointsXCoordinates.GetValueOrDefault(cp.UniqueIdentifier));

            var connectionPointsYCoordinate = _coordinatesCalculator.CalculateConnectionPointY(_columnId, table);
            newColumnPoints.ForEach(cp => cp.ConnectionPointTopCoordinate = connectionPointsYCoordinate);

            return newColumnPoints;
        }

        #endregion
    }
}
