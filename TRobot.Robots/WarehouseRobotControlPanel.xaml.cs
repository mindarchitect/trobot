using System.ComponentModel;
using System.Windows;
using TRobot.Robots.ViewModels;

namespace TRobot.Robots
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WarehouseRobotControlPanel : Window
    {
        private WarehouseRobotControlPanelViewModel warehouseRobotControlPanelViewModel;
        public WarehouseRobotControlPanel(WarehouseRobot robot)
        {
            InitializeComponent();

            warehouseRobotControlPanelViewModel = new WarehouseRobotControlPanelViewModel(robot);
            Closing += OnWindowClosing;

            DataContext = warehouseRobotControlPanelViewModel;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            //e.Cancel = true;
            warehouseRobotControlPanelViewModel.StopRobot();
            //Visibility = Visibility.Hidden;
        }
    }
}
