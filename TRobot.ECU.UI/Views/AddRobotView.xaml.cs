using System.Windows;
using TRobot.Core;
using TRobot.ECU.UI.ViewModels;

namespace TRobot.ECU.UI.Views
{
    /// <summary>
    /// Interaction logic for AddRobot.xaml
    /// </summary>
    public partial class AddRobotView : Window
    {
        public AddRobotView()
        {
            InitializeComponent();
            var addRobotViewModel = DependencyInjector.Resolve<AddRobotViewModel>();
            addRobotViewModel.View = this;

            DataContext = addRobotViewModel;
        }
    }
}
