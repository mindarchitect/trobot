using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Trajectory;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotTrajectoryValidationServiceClient
    {
        private IRobotTrajectoryValidationService validationService;        

        internal WarehouseRobotTrajectoryValidationServiceClient(Action<RobotValidationResult> validationResultCallback)
        {
            var callback = new WarehouseRobotTrajectoryValidationServiceCallback(validationResultCallback);
            var context = new InstanceContext(callback);
            //var pipeFactory = new DuplexChannelFactory<IRobotTrajectoryValidationService>(context, new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10000/ValidationService"));
            var pipeFactory = new DuplexChannelFactory<IRobotTrajectoryValidationService>(context, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/validation/ValidationService"));

            validationService = pipeFactory.CreateChannel();
        }

        internal void ValidateTrajectory(Guid robotId, List<Point> trajectoryPoints)
        {
            var robotTrajectory = new RobotDescartesTrajectory();
            robotTrajectory.RobotId = robotId;
            robotTrajectory.Trajectory = trajectoryPoints;

            validationService.ValidateRobotTrajectory(robotTrajectory);
        }        
    }
}
