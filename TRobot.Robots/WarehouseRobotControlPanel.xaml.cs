using System.Windows;
using TRobot.Robots.ViewModels;

namespace TRobot.Robots
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WarehouseRobotControlPanel : Window
    {
        public WarehouseRobotControlPanel(WarehouseRobot robot)
        {
            InitializeComponent();

            var warehouseRobotControlPanelViewModel = new WarehouseRobotControlPanelViewModel(robot);
            Closing += warehouseRobotControlPanelViewModel.OnWindowClosing;

            DataContext = warehouseRobotControlPanelViewModel;
        }

        private void Trajectory_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {

        }
    }
}
