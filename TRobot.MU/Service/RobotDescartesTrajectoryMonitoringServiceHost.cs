using System;
using System.ServiceModel;
using System.Windows;
using TRobot.Communication.Services;
using TRobot.Communication.Services.Monitoring;

namespace TRobot.MU.Service
{
    public class RobotDescartesTrajectoryMonitoringServiceHost: IServiceHostProvider
    {
        public IService Service { get; set; }

        private ServiceHost serviceHost;
        
        public RobotDescartesTrajectoryMonitoringServiceHost(IRobotTrajectoryMonitoringService robotTrajectoryMonitoringService)
        {
            Service = robotTrajectoryMonitoringService;

            try
            {
                serviceHost = new ServiceHost(Service, new Uri("net.pipe://localhost/monitoring"));
                serviceHost.Faulted += OnServiceHostFaulted;
                serviceHost.AddServiceEndpoint(typeof(IRobotTrajectoryMonitoringService), new NetNamedPipeBinding(), "MonitoringService");
                serviceHost.Open();
            }
            catch (TimeoutException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (CommunicationException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        private void OnServiceHostFaulted(object sender, EventArgs e)
        {
            serviceHost.Abort();
            serviceHost.Close();
        }

        public void Close()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }            
        }
    }
}
