using System.Windows;
using TRobot.Core.UI.ViewModels;

namespace TRobot.ECU.UI.ViewModels
{
    public abstract class BaseViewModel<V> : ViewModel where V : Window
    {
        public V View { get; set; }

        protected BaseViewModel()
        {
        }

        protected BaseViewModel(V view)
        {
            View = view;
        }
    }    
}
