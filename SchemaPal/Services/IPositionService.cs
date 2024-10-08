using SchemaPal.DataTransferObjects;
using SchemaPal.Pages.SchemaMakerHelpers;

namespace SchemaPal.Services
{
    public interface IPositionService
    {
        void UpdateTableCoordinates(
            DatabaseSchema databaseSchema,
            DragAndDropHelper dragAndDropData,
            (double NewClientX, double NewClientY) newClientPosition);

        void UpdateRelationshipPositions(
            DatabaseSchema databaseSchema,
            List<int> tableIds = null);
    }
}
