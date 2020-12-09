﻿using System;
using System.ServiceModel;
using TRobot.Core.Services.Contracts.Data;
using TRobot.Communication.Events;

namespace TRobot.Communication.Services.Monitoring
{    
    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IRobotTrajectoryMonitoringServiceCallback),
        Name = "IRobotTrajectoryMonitoringService",
        Namespace = "http://trobot/monitoring"
    )]
    public interface IRobotTrajectoryMonitoringService : IService
    {
        [OperationContract]  
        void SetupRobotTrajectory(RobotDescartesTrajectory robotTrajectory);

        [OperationContract]
        void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotTrajectory);

        event EventHandler<TrajectorySetEventArguments> RobotTrajectorySet;
        event EventHandler<RobotPositionUpdatedEventArguments> RobotPositionUpdated;
    }
}
