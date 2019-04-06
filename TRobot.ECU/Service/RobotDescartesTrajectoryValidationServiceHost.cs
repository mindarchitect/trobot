using System;
using System.ServiceModel;
using System.Windows;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;
using TRobot.ECU.UI.Service;

namespace TRobot.ECU.Service
{
    public class RobotDescartesTrajectoryValidationServiceHost
    {
        public RobotDescartesTrajectoryValidationServiceHost(DescartesRobotFactory descartesRobotFactory)
        { 
            try
            {
                var host = new ServiceHost(new RobotDescartesTrajectoryValidationService(descartesRobotFactory), new Uri("net.pipe://localhost/validation"));
                host.AddServiceEndpoint(typeof(IRobotTrajectoryValidationService), new NetNamedPipeBinding(), "ValidationService");
                host.Open();
            }                       
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
