using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
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
            var robotTrajectory = new RobotDescartesTrajectoryPoints();
            robotTrajectory.RobotId = robotId;
            robotTrajectory.TrajectoryPoints = trajectoryPoints;

            service.ValidateRobotTrajectory(robotTrajectory);
        }
    }
}
