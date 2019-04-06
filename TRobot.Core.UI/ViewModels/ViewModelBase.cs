using System.ComponentModel;

namespace TRobot.Core.UI.ViewModels
{
    public class ViewModelBase<ViewType> : INotifyPropertyChanged where ViewType : IView
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ViewType view;
        public ViewType View
        {
            get
            {
                return view;
            }
        }
        public ViewModelBase(ViewType view)
        {
            this.view = view;
            View.DataContext = this;
        }
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
