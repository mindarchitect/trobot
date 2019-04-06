using System;
using System.ServiceModel;
using System.Windows;
using TRobot.Communication.Services.Monitoring;

namespace TRobot.MU.Service
{
    public class RobotDescartesTrajectoryMonitoringServiceHost
    {
        public IRobotTrajectoryMonitoringService Service { get; private set; }
        public RobotDescartesTrajectoryMonitoringServiceHost()
        { 
            try
            {
                Service = new RobotDescartesTrajectoryMonitoringService();
                var host = new ServiceHost(Service, new Uri("net.pipe://localhost/monitoring"));
                host.AddServiceEndpoint(typeof(IRobotTrajectoryMonitoringService), new NetNamedPipeBinding(), "MonitoringService");
                host.Open();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
    }
}
