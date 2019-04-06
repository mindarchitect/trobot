using System;
using System.ServiceModel;
using TRobot.Communication.Trajectory;
using TRobot.Core;
using TRobot.ECU.UI.Service;

namespace TRobot.ECU.Service
{
    public class RobotDescartesTrajectoryValidationServiceHost
    {
        public RobotDescartesTrajectoryValidationServiceHost(DescartesRobotFactory descartesRobotFactory)
        {
            //var host = new ServiceHost(new RobotDescartesTrajectoryValidationService(descartesRobotFactory), new Uri("net.tcp://localhost:10000"));
            var host = new ServiceHost(new RobotDescartesTrajectoryValidationService(descartesRobotFactory), new Uri("net.pipe://localhost/validation"));
            host.AddServiceEndpoint(typeof(IRobotTrajectoryValidationService), new NetNamedPipeBinding(), "ValidationService");
            host.Open();
        }
    }
}
