using System.Windows;
using TRobot.Robots.ViewModels;

namespace TRobot.Robots.Views
{
    /// <summary>
    /// Interaktionslogik für AddDestinationPoint.xaml
    /// </summary>
    public partial class AddDestinationPointView : Window
    {
        public AddDestinationPointView(WarehouseRobotControlPanelViewModel warehouseRobotControlPanelViewModel)
        {
            InitializeComponent();

            var addDestinationPointViewModel = new AddDestinationPointViewModel(warehouseRobotControlPanelViewModel);
            DataContext = addDestinationPointViewModel;
        }
    }
}
