using System;
using System.ServiceModel;
using TRobot.Communication.Trajectory;

namespace TRobot.MU.Service
{
    public class RobotDescartesTrajectoryMonitoringServiceHost
    {
        public RobotDescartesTrajectoryMonitoringServiceHost()
        {
            //var host = new ServiceHost(typeof(RobotDescartesTrajectoryMonitoringService), new Uri("net.tcp://localhost:10001"));
            var host = new ServiceHost(typeof(RobotDescartesTrajectoryMonitoringService), new Uri("net.pipe://localhost/monitoring"));
            host.AddServiceEndpoint(typeof(IRobotTrajectoryMonitoringService), new NetNamedPipeBinding(), "MonitoringService");            
            host.Open();
        }
    }
}
