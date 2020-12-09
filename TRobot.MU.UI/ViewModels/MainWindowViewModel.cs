
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

namespace TRobot.MU.UI.ViewModels
{
    class MainWindowViewModel: BaseViewModel
    {
        public ObservableCollection<RobotMonitoringItem> RobotMonitoringItems { get; private set; }
        private IServiceHostProvider serviceHostProvider;

        public MainWindowViewModel(IServiceHostProvider serviceHostProvider)
        {
            this.serviceHostProvider = serviceHostProvider;
            RobotMonitoringItems = new ObservableCollection<RobotMonitoringItem>();

            if (serviceHostProvider.Service is IRobotTrajectoryMonitoringService)
            {
                IRobotTrajectoryMonitoringService robotTrajectoryMonitoringService = (IRobotTrajectoryMonitoringService) serviceHostProvider.Service;

                robotTrajectoryMonitoringService.RobotTrajectorySet += OnServiceRobotTrajectorySet;
                robotTrajectoryMonitoringService.RobotPositionUpdated += OnServiceRobotPositionUpdated;
            }            
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

        private void OnServiceRobotTrajectorySet(object sender, TrajectorySetEventArguments e)
        {
            var robotId = e.RobotDescartesTrajectory.RobotId;
            var item = RobotMonitoringItems.FirstOrDefault(robotMonitoringItem => robotMonitoringItem.Guid == robotId);

            if (item != null)
            {
                item.StartPoint = e.RobotDescartesTrajectory.CurrentPosition;
                item.Trajectory = new PointCollection(e.RobotDescartesTrajectory.Trajectory);                
                item.CurrentPosition = e.RobotDescartesTrajectory.CurrentPosition;
                item.Title = e.RobotDescartesTrajectory.RobotTitle;                
            }
            else 
            {
                var random = new Random();
                var robotMonitoringItem = new RobotMonitoringItem
                {
                    StartPoint = e.RobotDescartesTrajectory.CurrentPosition,
                    Trajectory = new PointCollection(e.RobotDescartesTrajectory.Trajectory),
                    Color = Color.FromRgb(Convert.ToByte(random.Next(256)), Convert.ToByte(random.Next(256)), Convert.ToByte(random.Next(256))),
                    CurrentPosition = e.RobotDescartesTrajectory.CurrentPosition,
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
