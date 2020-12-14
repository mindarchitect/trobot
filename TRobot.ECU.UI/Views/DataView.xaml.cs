using System.Windows;
using TRobot.Core;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaktionslogik für DataView.xaml
    /// </summary>
    public partial class DataView : Window
    {
        public DataView()
        {
            InitializeComponent();

            var dataViewModel = DependencyInjector.Resolve<DataViewModel>();
            dataViewModel.View = this;

            DataContext = dataViewModel;
        }
    }
}
