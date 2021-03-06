﻿using System;
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

                Channel.SetupRobotTrajectory(robotTrajectory);
            }            
        }

        internal void UpdateRobotPosition(Guid robotId, Point position)
        {
            if (InnerDuplexChannel.State != CommunicationState.Faulted)
            {
                var robotTrajectoryPosition = new RobotDescartesTrajectoryPosition();
                robotTrajectoryPosition.RobotId = robotId;
                robotTrajectoryPosition.CurrentPosition = position;

                Channel.UpdateRobotPosition(robotTrajectoryPosition);
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
