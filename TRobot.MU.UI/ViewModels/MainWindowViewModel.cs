
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using TRobot.Communication.Events;
using TRobot.ECU.UI.ViewModels;
using TRobot.MU.UI.Models;
using System.Linq;
using System;
using TRobot.Communication.Services.Monitoring;
using TRobot.Communication.Services;
using System.Windows;

namespace TRobot.MU.UI.ViewModels
{
    class MainWindowViewModel: BaseViewModel
    {
        public ObservableCollection<RobotMonitoringItem> RobotMonitoringItems { get; private set; }
        private IServiceHostProvider<IRobotTrajectoryMonitoringService> serviceHostProvider;

        public MainWindowViewModel(IServiceHostProvider<IRobotTrajectoryMonitoringService> serviceHostProvider)
        {
            this.serviceHostProvider = serviceHostProvider;
            RobotMonitoringItems = new ObservableCollection<RobotMonitoringItem>();

            serviceHostProvider.Service.RobotTrajectorySet += OnServiceRobotTrajectorySet;
            serviceHostProvider.Service.RobotPositionUpdated += OnServiceRobotPositionUpdated;
            serviceHostProvider.Service.RobotPositionReset += OnServiceRobotPositionReset;
        }

        private void OnServiceRobotPositionUpdated(object sender, RobotPositionUpdatedEventArguments e)
        {
            var robotId = e.RobotDescartesTrajectoryPosition.RobotId;
            var item = RobotMonitoringItems.FirstOrDefault(robotMonitoringItem => robotMonitoringItem.Guid == robotId);

            if (item != null)
            {
                item.CurrentPosition = e.RobotDescartesTrajectoryPosition.CurrentPosition;
            }            
        }

        private void OnServiceRobotPositionReset(object sender, RobotPositionResetEventArguments e)
        {
            var robotId = e.Robot.RobotId;
            var item = RobotMonitoringItems.FirstOrDefault(robotMonitoringItem => robotMonitoringItem.Guid == robotId);

            if (item != null)
            {
                item.CurrentPosition = new Point(0,0);
            }
        }

        private void OnServiceRobotTrajectorySet(object sender, TrajectorySetEventArguments e)
        {
            var robotId = e.RobotDescartesTrajectory.RobotId;
            var item = RobotMonitoringItems.FirstOrDefault(robotMonitoringItem => robotMonitoringItem.Guid == robotId);

            if (item != null)
            {
                item.StartPoint = e.RobotDescartesTrajectory.Trajectory.First();
                item.Trajectory = new PointCollection(e.RobotDescartesTrajectory.Trajectory);
                item.Title = e.RobotDescartesTrajectory.RobotTitle;                
            }
            else 
            {
                var random = new Random();
                var robotMonitoringItem = new RobotMonitoringItem
                {
                    StartPoint = e.RobotDescartesTrajectory.Trajectory.First(),
                    Trajectory = new PointCollection(e.RobotDescartesTrajectory.Trajectory),
                    Color = Color.FromRgb(Convert.ToByte(random.Next(256)), Convert.ToByte(random.Next(256)), Convert.ToByte(random.Next(256))),
                    Guid = e.RobotDescartesTrajectory.RobotId,
                    Title = e.RobotDescartesTrajectory.RobotTitle
                };

                RobotMonitoringItems.Add(robotMonitoringItem);
            }            
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            serviceHostProvider.Close();
        }
    }
}
