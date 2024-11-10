using SchemaPal.Enums;

namespace SchemaPal.Services.HelperServices
{
    public interface IComponentActionsStorage
    {
        event Func<Task> OnUserSessionTypeChange;

        Task InvokeUserSessionTypeChange();

        event Func<int, SidebarExpandableElementType, Task> OnCanvasComponentDoubleClick;

        Task InvokeCanvasComponentDoubleClick(int elementId, SidebarExpandableElementType elementType);
    }
}
