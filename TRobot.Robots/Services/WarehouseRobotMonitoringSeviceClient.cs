using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using TRobot.Core.Services.Contracts.Data;
using TRobot.Communication.Services.Monitoring;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotMonitoringSeviceClient : DuplexClientBase<IRobotTrajectoryMonitoringService>
    {
        public WarehouseRobotMonitoringSeviceClient(object callbackInstance, Binding binding, EndpointAddress remoteAddress)
        : base(callbackInstance, binding, remoteAddress)
        {
        }

        internal void SetupTrajectory(Guid robotId, string robotTitle, List<Point> trajectoryPoints)
        {
            if (InnerChannel.State != CommunicationState.Faulted)
            {
                var robotTrajectory = new RobotDescartesTrajectory();
                robotTrajectory.RobotId = robotId;
                robotTrajectory.RobotTitle = robotTitle;
                robotTrajectory.Trajectory = trajectoryPoints;
                //robotTrajectory.CurrentPosition = trajectoryPoints[0];

                Channel.SetupRobotTrajectory(robotTrajectory);
            }            
        }

        internal void UpdatePosition(Guid robotId, Point position)
        {
            if (InnerChannel.State != CommunicationState.Faulted)
            {
                var robotTrajectoryPosition = new RobotDescartesTrajectoryPosition();
                robotTrajectoryPosition.RobotId = robotId;
                robotTrajectoryPosition.CurrentPosition = position;

                Channel.UpdateRobotPosition(robotTrajectoryPosition);
            }
        }
    }
}
