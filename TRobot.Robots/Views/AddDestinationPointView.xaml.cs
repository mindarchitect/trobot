using System.Windows;
using TRobot.Robots.ViewModels;

namespace TRobot.Robots.Views
{
    /// <summary>
    /// Interaktionslogik für AddDestinationPoint.xaml
    /// </summary>
    partial class AddDestinationPointView : Window
    {
        internal AddDestinationPointView(WarehouseRobotControlPanelViewModel warehouseRobotControlPanelViewModel)
        {
            InitializeComponent();

            var addDestinationPointViewModel = new AddDestinationPointViewModel(warehouseRobotControlPanelViewModel);
            DataContext = addDestinationPointViewModel;
        }
    }
}
