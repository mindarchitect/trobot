﻿using System;
using System.ServiceModel;
using System.Windows;
using TRobot.Communication.Services.Trajectory;
using TRobot.Core;
using TRobot.Core.Robot.Interfaces;
using TRobot.ECU.UI.Service;

namespace TRobot.ECU.Service
{
    public class RobotDescartesTrajectoryValidationServiceHost : IServiceHostProvider
    {
        private ServiceHost serviceHost;
        public RobotDescartesTrajectoryValidationServiceHost(DescartesRobotFactory descartesRobotFactory)
        {                       
            try
            {
                serviceHost = new ServiceHost(new RobotDescartesTrajectoryValidationService(descartesRobotFactory), new Uri("net.pipe://localhost/validation"));
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
