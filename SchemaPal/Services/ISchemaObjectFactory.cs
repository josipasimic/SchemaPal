using SchemaPal.DataTransferObjects;

namespace SchemaPal.Services
{
    public interface ISchemaObjectFactory
    {
        void CreateNewTable(DatabaseSchema databaseSchema);

        Relationship CreateNewRelationship(ConnectionPoint startingConnectionPoint,
            Dictionary<string, string> connectionPointColors);

        void CloseNewRelationship(DatabaseSchema databaseSchema,
            Relationship newRelationship,
            ConnectionPoint endingConnectionPoint,
            string startingConnectionPointId);
    }
}
