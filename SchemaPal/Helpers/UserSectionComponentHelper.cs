namespace SchemaPal.Helpers
{
    public static class UserSectionComponentHelper
    {
        public static event Func<Task> OnUserStatusChange;

        public static async Task NotifyUserStatusChange()
        {
            if (OnUserStatusChange != null)
            {
                await OnUserStatusChange.Invoke();
            }
        }
    }
}
