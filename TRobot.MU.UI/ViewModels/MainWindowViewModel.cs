
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using TRobot.MU.Service;
using TRobot.MU.UI.Models;

namespace TRobot.MU.UI.ViewModels
{
    public class MainWindowViewModel
    {
        public IList<RobotMonitoringItem> Robots { get; private set; }

        private RobotDescartesTrajectoryMonitoringServiceHost trajectoryMonitoringServiceHost;

        public MainWindowViewModel()
        {
            trajectoryMonitoringServiceHost = new RobotDescartesTrajectoryMonitoringServiceHost();

            Robots = new List<RobotMonitoringItem>();

            var robot1Trajectory = new PointCollection();

            robot1Trajectory.Add(new Point(10, 10));
            robot1Trajectory.Add(new Point(20, 40));
            robot1Trajectory.Add(new Point(40, 70));            

            var robotMonitoringItem1 = new RobotMonitoringItem();
            robotMonitoringItem1.StartPoint = new Point(0, 0);
            robotMonitoringItem1.Trajectory = robot1Trajectory;
            robotMonitoringItem1.Color = Colors.DarkBlue;
            robotMonitoringItem1.CurrentPosition = new Point(30, 50);
            robotMonitoringItem1.Guid = Guid.NewGuid();
            robotMonitoringItem1.Title = "Robot 1";            

            Robots.Add(robotMonitoringItem1);
        }
    }
}
