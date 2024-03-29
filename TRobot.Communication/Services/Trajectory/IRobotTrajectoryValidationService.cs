﻿using System;
using System.ServiceModel;
using TRobot.Core;
using TRobot.Core.Services;
using TRobot.Core.Services.Contracts.Data;

namespace TRobot.Communication.Services.Trajectory
{    
    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IRobotTrajectoryValidationServiceCallback),
        Name = "Validation Service",
        Namespace = "http://trobot/validation"
    )]
    public interface IRobotTrajectoryValidationService : IService
    {
        [OperationContract]  
        void ValidateRobotTrajectory(RobotDescartesTrajectory robotTrajectory);
    }
}
