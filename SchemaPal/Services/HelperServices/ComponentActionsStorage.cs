using SchemaPal.Enums;

namespace SchemaPal.Services.HelperServices
{
    public class ComponentActionsStorage : IComponentActionsStorage
    {
        public event Func<Task> OnUserSessionTypeChange;

        public async Task InvokeUserSessionTypeChange()
        {
            if (OnUserSessionTypeChange != null)
            {
                await OnUserSessionTypeChange.Invoke();
            }
        }

        public event Func<int, SidebarExpandableElementType, Task> OnCanvasComponentDoubleClick;

        public async Task InvokeCanvasComponentDoubleClick(int elementId, SidebarExpandableElementType elementType)
        {
            if (OnCanvasComponentDoubleClick != null)
            {
                await OnCanvasComponentDoubleClick.Invoke(elementId, elementType);
            }
        }
    }
}
