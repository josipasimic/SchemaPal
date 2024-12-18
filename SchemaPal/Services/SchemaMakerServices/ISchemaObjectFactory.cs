﻿using SchemaPal.SchemaElements;

namespace SchemaPal.Services.SchemaMakerServices
{
    public interface ISchemaObjectFactory
    {
        void SetWithExistingSchema(DatabaseSchema databaseSchema);

        int CreateNewTable(DatabaseSchema databaseSchema);

        Relationship CreateNewRelationship(ConnectionPoint startingConnectionPoint);

        void CloseNewRelationship(DatabaseSchema databaseSchema,
            Relationship newRelationship,
            ConnectionPoint endingConnectionPoint,
            string startingConnectionPointId);

        void CreateNewColumn(DatabaseSchema databaseSchema, int tableId);

        void CreateNewIndex(Table table);

        void UpdateColumnsOnIndex(Table table, int indexId, List<int> columnIds);

        HashSet<int> DeleteTable(DatabaseSchema databaseSchema, int tableId);

        HashSet<int> DeleteColumn(DatabaseSchema databaseSchema, int tableId, int columnId);

        void DeleteIndexes(DatabaseSchema databaseSchema,
            int tableId,
            int? indexId = null,
            int? columnId = null);

        HashSet<int> DeleteRelationships(DatabaseSchema databaseSchema,
            int? relationshipId = null,
            int? tableId = null,
            int? columnId = null);
    }
}
