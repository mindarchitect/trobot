using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
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
            Closing += OnClosing;

            DataContext = warehouseRobotControlPanelViewModel;

            Uri iconUri = new Uri("pack://application:,,,/Images/control.jpg", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            warehouseRobotControlPanelViewModel.Robot.Stop();
        }
    }
}
