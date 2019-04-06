using System;
using System.ServiceModel;
using TRobot.Communication.Trajectory;

namespace TRobot.MU.Service
{
    public class RobotDescartesTrajectoryMonitoringServiceHost
    {
        public RobotDescartesTrajectoryMonitoringServiceHost()
        {
            var host = new ServiceHost(typeof(RobotDescartesTrajectoryMonitoringService), new Uri("net.pipe://localhost"));
            host.AddServiceEndpoint(typeof(IRobotTrajectoryMonitoringService), new NetNamedPipeBinding(), "PlotRobotTrajectory");
            host.Open();
        }
    }
}
