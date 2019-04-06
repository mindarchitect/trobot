namespace TRobot.Core.UI.ViewModels
{
    public interface IView
    {
        object DataContext { get; set; }
        void Close();
    }
}
