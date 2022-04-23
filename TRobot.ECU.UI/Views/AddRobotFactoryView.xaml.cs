using System.Windows;
using TRobot.Core;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaction logic for AddRobotFactoryView.xaml
    /// </summary>
    public partial class AddRobotFactoryView : Window
    {
        public AddRobotFactoryView()
        {
            InitializeComponent();

            var addFactoryViewModel = DependencyInjector.Resolve<AddRobotFactoryViewModel>();
            addFactoryViewModel.View = this;

            DataContext = addFactoryViewModel;
        }
    }
}
