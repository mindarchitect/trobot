
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using TRobot.Communication.Events;
using TRobot.ECU.UI.ViewModels;
using TRobot.MU.Service;
using TRobot.MU.UI.Models;

namespace TRobot.MU.UI.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {
        public ObservableCollection<RobotMonitoringItem> Robots { get; private set; }

        private RobotDescartesTrajectoryMonitoringServiceHost trajectoryMonitoringServiceHost;

        public MainWindowViewModel()
        {
            trajectoryMonitoringServiceHost = new RobotDescartesTrajectoryMonitoringServiceHost();
            Robots = new ObservableCollection<RobotMonitoringItem>();

            trajectoryMonitoringServiceHost.Service.RobotTrajectorySet += OnServiceRobotTrajectorySet;
            trajectoryMonitoringServiceHost.Service.RobotPositionUpdated += OnServiceRobotPositionUpdated;
        }

        private void OnServiceRobotPositionUpdated(object sender, RobotPositionUpdatedEventArguments e)
        {
            for (var i = 0; i < Robots.Count; i++)
            {
                var robot = Robots[i];

                if (robot.Guid == e.RobotDescartesTrajectoryPosition.RobotId)
                {
                    robot.CurrentPosition = e.RobotDescartesTrajectoryPosition.CurrentPosition;
                }
            }
        }

        private void OnServiceRobotTrajectorySet(object sender, TrajectorySetEventArguments e)
        {            
            var robotMonitoringItem1 = new RobotMonitoringItem();

            robotMonitoringItem1.StartPoint = e.RobotDescartesTrajectory.CurrentPosition;
            robotMonitoringItem1.Trajectory = new PointCollection(e.RobotDescartesTrajectory.Trajectory);
            robotMonitoringItem1.Color = Colors.DarkBlue;
            robotMonitoringItem1.CurrentPosition = e.RobotDescartesTrajectory.CurrentPosition;
            robotMonitoringItem1.Guid = e.RobotDescartesTrajectory.RobotId;
            robotMonitoringItem1.Title = e.RobotDescartesTrajectory.RobotTitle;

            Robots.Add(robotMonitoringItem1);
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            trajectoryMonitoringServiceHost.Close();
        }
    }
}
