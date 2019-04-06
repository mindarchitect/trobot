
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using TRobot.MU.UI.Models;

namespace TRobot.MU.UI.ViewModels
{
    public class MainWindowViewModel
    {
        public IList<RobotMonitoringItem> Robots { get; private set; }

        public MainWindowViewModel()
        {
            var robotMonitoringItem1 = new RobotMonitoringItem();
            robotMonitoringItem1.Trajectory = new LinkedList<Vector>();
            robotMonitoringItem1.Color = Colors.DarkBlue;

            Robots.Add(robotMonitoringItem1);
        }
    }
}
