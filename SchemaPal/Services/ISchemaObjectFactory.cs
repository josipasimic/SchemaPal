using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services
{
    public interface ISchemaObjectFactory
    {
        int CreateNewTable(DatabaseSchema databaseSchema);

        Relationship CreateNewRelationship(ConnectionPoint startingConnectionPoint);

        void CloseNewRelationship(DatabaseSchema databaseSchema,
            Relationship newRelationship,
            ConnectionPoint endingConnectionPoint,
            string startingConnectionPointId);
    }
}
