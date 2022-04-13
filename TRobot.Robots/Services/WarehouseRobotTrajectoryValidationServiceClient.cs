using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using TRobot.Core.Services.Contracts.Data;
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
            if (InnerChannel.State != CommunicationState.Faulted)
            { 
                var robotTrajectory = new RobotDescartesTrajectory();
                robotTrajectory.RobotId = robotId;
                robotTrajectory.Trajectory = trajectoryPoints;

                Channel.ValidateRobotTrajectory(robotTrajectory);
            }
            else
            {                
                MessageBox.Show(string.Format("Channel is in faulted state: {0}", Channel.ToString()), "Trajectory validation error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
