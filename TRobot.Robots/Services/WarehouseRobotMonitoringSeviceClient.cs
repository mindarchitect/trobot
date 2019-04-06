using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Trajectory;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotMonitoringSeviceClient : ClientBase<IRobotTrajectoryMonitoringService>
    {
        public WarehouseRobotMonitoringSeviceClient()
        {
        }

        public WarehouseRobotMonitoringSeviceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
        {
        }

        public WarehouseRobotMonitoringSeviceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WarehouseRobotMonitoringSeviceClient(string endpointConfigurationName, EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public WarehouseRobotMonitoringSeviceClient(Binding binding, EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
        {
        }

        internal void SetupTrajectory(Guid robotId, List<Point> trajectoryPoints)
        {
            var robotTrajectory = new RobotDescartesTrajectory();
            robotTrajectory.RobotId = robotId;
            robotTrajectory.Trajectory = trajectoryPoints;

            Channel.SetupRobotTrajectory(robotTrajectory);
        }

        internal void UpdatePosition(Guid robotId, Point position)
        {
            var robotTrajectoryPosition = new RobotDescartesTrajectoryPosition();
            robotTrajectoryPosition.RobotId = robotId;
            robotTrajectoryPosition.CurrentPosition = position;

            Channel.UpdateRobotPosition(robotTrajectoryPosition);
        }
    }
}
