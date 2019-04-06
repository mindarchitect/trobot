using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Trajectory;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotTrajectoryValidationServiceClient
    {
        private IRobotTrajectoryValidationService service;        
        internal WarehouseRobotTrajectoryValidationServiceClient(Action<RobotValidationResult> validationResultCallback)
        {
            var callback = new WarehouseRobotTrajectoryValidationCallback(validationResultCallback);
            var context = new InstanceContext(callback);
            var pipeFactory = new DuplexChannelFactory<IRobotTrajectoryValidationService>(context, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/ValidateRobotTrajectory"));

            service = pipeFactory.CreateChannel();            
        }

        internal void ValidateTrajectory(Guid robotId, List<Point> trajectoryPoints)
        {
            var robotTrajectory = new RobotDescartesTrajectory();
            robotTrajectory.RobotId = robotId;
            robotTrajectory.Trajectory = new PointCollection(trajectoryPoints);

            service.ValidateRobotTrajectory(robotTrajectory);
        }
    }
}
