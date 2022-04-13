using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using TRobot.Communication.Events;
using System.Linq;
using System;
using TRobot.Communication.Services.Monitoring;
using TRobot.Communication.Services;
using TRobot.MU.UI.Views;
using TRobot.ECU.UI.ViewModels;
using TRobot.MU.UI.Models;

namespace TRobot.MU.UI.ViewModels
{
    class MainWindowViewModel: BaseViewModel<MainWindowView>
    {
        public ObservableCollection<RobotMonitoringItemModel> RobotMonitoringItems { get; private set; }
        private IServiceHostProvider<IRobotTrajectoryMonitoringService> serviceHostProvider;

        public MainWindowViewModel(IServiceHostProvider<IRobotTrajectoryMonitoringService> serviceHostProvider)
        {
            this.serviceHostProvider = serviceHostProvider;
            RobotMonitoringItems = new ObservableCollection<RobotMonitoringItemModel>();

            serviceHostProvider.Service.RobotTrajectorySet += OnServiceRobotTrajectorySet;
            serviceHostProvider.Service.RobotTrajectoryCleaned += OnServiceRobotTrajectoryCleaned;
            serviceHostProvider.Service.RobotPositionUpdated += OnServiceRobotPositionUpdated;
            serviceHostProvider.Service.TestEvent += OnTestEventHanler;
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

        private void OnTestEventHanler(object sender, RobotTestEventArguments e)
        {
            var robotId = e.Robot.RobotId;
            var item = RobotMonitoringItems.FirstOrDefault(robotMonitoringItem => robotMonitoringItem.Guid == robotId);

            if (item != null)
            {            
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
                var robotMonitoringItem = new RobotMonitoringItemModel
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

        private void OnServiceRobotTrajectoryCleaned(object sender, TrajectoryCleanedEventArguments e)
        {
            var robotId = e.RobotId;
            var item = RobotMonitoringItems.FirstOrDefault(robotMonitoringItem => robotMonitoringItem.Guid == robotId);

            if (item != null)
            {
                RobotMonitoringItems.Remove(item);
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            serviceHostProvider.Close();
        }
    }
}
