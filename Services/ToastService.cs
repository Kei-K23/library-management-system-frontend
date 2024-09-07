namespace LibraryManagementSystemApp.Services
{
    public interface IToastService
    {
        event Action<string, string> OnShow;
        event Action OnHide;
        void ShowToast(string message, string type);
        void HideToast();
    }

    public class ToastService : IToastService
    {
        public event Action<string, string> OnShow;
        public event Action OnHide;

        public void ShowToast(string message, string type = "success")
        {
            OnShow?.Invoke(message, type);
        }

        public void HideToast()
        {
            OnHide?.Invoke();
        }
    }
}
