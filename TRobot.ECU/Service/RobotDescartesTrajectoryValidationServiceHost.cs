using System;
using System.ServiceModel;
using System.Windows;
using TRobot.Communication.Services;
using TRobot.Communication.Services.Trajectory;
using Unity;

namespace TRobot.ECU.Service
{
    public class RobotDescartesTrajectoryValidationServiceHost : IServiceHostProvider
    {
        public IService Service { get; set; }

        private ServiceHost serviceHost;
        
        public RobotDescartesTrajectoryValidationServiceHost(IRobotTrajectoryValidationService robotTrajectoryValidationService)
        {
            Service = robotTrajectoryValidationService;

            try
            {
                serviceHost = new ServiceHost(robotTrajectoryValidationService, new Uri("net.pipe://localhost/validation"));
                serviceHost.Faulted += OnServiceHostFaulted;
                serviceHost.AddServiceEndpoint(typeof(IRobotTrajectoryValidationService), new NetNamedPipeBinding(), "ValidationService");
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

        public void Close()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
        }

        private void OnServiceHostFaulted(object sender, EventArgs e)
        {
            serviceHost.Abort();
            serviceHost.Close();
        }
    }
}
