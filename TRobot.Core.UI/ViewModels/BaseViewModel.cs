using System.Windows.Controls;
using TRobot.Core.UI.ViewModels;

namespace TRobot.ECU.UI.ViewModels
{
    public abstract class BaseViewModel<V> : ViewModel where V : Control
    {
        private readonly V view;
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
