﻿
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using TRobot.Communication.Events;
using TRobot.ECU.UI.ViewModels;
using TRobot.MU.Service;
using TRobot.MU.UI.Models;
using System.Linq;
using System;

namespace TRobot.MU.UI.ViewModels
{
    class MainWindowViewModel: BaseViewModel
    {
        public ObservableCollection<RobotMonitoringItem> RobotMonitoringItems { get; private set; }
        private RobotDescartesTrajectoryMonitoringServiceHost trajectoryMonitoringServiceHost;

        public MainWindowViewModel()
        {
            trajectoryMonitoringServiceHost = new RobotDescartesTrajectoryMonitoringServiceHost();
            RobotMonitoringItems = new ObservableCollection<RobotMonitoringItem>();

            trajectoryMonitoringServiceHost.Service.RobotTrajectorySet += OnServiceRobotTrajectorySet;
            trajectoryMonitoringServiceHost.Service.RobotPositionUpdated += OnServiceRobotPositionUpdated;
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
            trajectoryMonitoringServiceHost.Close();
        }
    }
}
