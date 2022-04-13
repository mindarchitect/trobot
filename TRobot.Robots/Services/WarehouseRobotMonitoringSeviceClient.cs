using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using TRobot.Core.Services.Contracts.Data;
using TRobot.Communication.Services.Monitoring;

namespace TRobot.Robots.Services
{
    internal class WarehouseRobotMonitoringSeviceClient : DuplexClientBase<IRobotTrajectoryMonitoringService>
    {
        public WarehouseRobotMonitoringSeviceClient(object callbackInstance, Binding binding, EndpointAddress remoteAddress)
        : base(callbackInstance, binding, remoteAddress)
        {
        }       

        internal void SetupRobotTrajectory(Guid robotId, string robotTitle, List<Point> trajectoryPoints)
        {
            if (InnerDuplexChannel.State != CommunicationState.Faulted)
            {
                var robotTrajectory = new RobotDescartesTrajectory();
                robotTrajectory.RobotId = robotId;
                robotTrajectory.RobotTitle = robotTitle;
                robotTrajectory.Trajectory = trajectoryPoints;

                try
                {
                    Channel.SetupRobotTrajectory(robotTrajectory);
                }
                catch (EndpointNotFoundException e)
                {
                    MessageBox.Show(string.Format("{0}\n\n{1}", e.Message, "Please, launch trajectory monitoring tool!"), "Channel is in faulted state", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Channel is in faulted state.\nPlease, restart robot control panel!", "Setup robot trajectory operation error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        internal void ClearTrajectory(Guid robotId)
        {
            if (InnerDuplexChannel.State != CommunicationState.Faulted)
            {
                try
                {
                    Channel.ClearRobotTrajectory(robotId);
                }
                catch (EndpointNotFoundException e)
                {
                    MessageBox.Show(string.Format("{0}\n\n{1}", e.Message, "Please, launch trajectory monitoring tool!"), "Channel is in faulted state", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Channel is in faulted state.\nPlease, restart robot control panel!", "Clear trajectory operation error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        internal void UpdateRobotPosition(Guid robotId, Point position)
        {
            if (InnerDuplexChannel.State != CommunicationState.Faulted)
            {
                var robotTrajectoryPosition = new RobotDescartesTrajectoryPosition();
                robotTrajectoryPosition.RobotId = robotId;
                robotTrajectoryPosition.CurrentPosition = position;

                try
                {
                    Channel.UpdateRobotPosition(robotTrajectoryPosition);
                }
                catch (EndpointNotFoundException e)
                {
                    MessageBox.Show(string.Format("{0}\n\n{1}", e.Message, "Please, launch trajectory monitoring tool!"), "Channel is in faulted state", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Channel is in faulted state.\nPlease, restart robot control panel!", "Update robot position operation error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        internal void TestOperation(Guid robotId)
        {
            if (InnerDuplexChannel.State != CommunicationState.Faulted)
            {
                var robot = new Robot();
                robot.RobotId = robotId;

                Channel.TestOperation(robot);
            }
        }
    }
}
