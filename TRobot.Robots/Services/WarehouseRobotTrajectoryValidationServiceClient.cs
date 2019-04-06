using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Services.Trajectory;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotTrajectoryValidationServiceClient : DuplexClientBase<IRobotTrajectoryValidationService>
    {
        public WarehouseRobotTrajectoryValidationServiceClient(object callbackInstance, Binding binding, EndpointAddress remoteAddress)
        : base(callbackInstance, binding, remoteAddress)
        {
        }        

        internal void ValidateTrajectory(Guid robotId, List<Point> trajectoryPoints)
        {
            var robotTrajectory = new RobotDescartesTrajectory();
            robotTrajectory.RobotId = robotId;
            robotTrajectory.Trajectory = trajectoryPoints;

            Channel.ValidateRobotTrajectory(robotTrajectory);
        }        
    }
}
