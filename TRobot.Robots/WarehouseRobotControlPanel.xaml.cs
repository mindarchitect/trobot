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

            DataContext = new WarehouseRobotControlPanelViewModel(robot);
        }
    }
}
