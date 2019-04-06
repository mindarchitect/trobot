using System;
using System.ServiceModel;
using TRobot.Communication.Trajectory;
using TRobot.MU.UI.Service;

namespace TRobot.ECU.Service
{
    public class RobotDescartesTrajectoryValidationServiceHost
    {
        public RobotDescartesTrajectoryValidationServiceHost()
        {
            var host = new ServiceHost(typeof(RobotDescartesTrajectoryMonitoringService), new Uri("net.pipe://localhost"));
            host.AddServiceEndpoint(typeof(IRobotTrajectoryMonitoringService), new NetNamedPipeBinding(), "PlotRobotTrajectory");
            host.Open();
        }
    }
}
