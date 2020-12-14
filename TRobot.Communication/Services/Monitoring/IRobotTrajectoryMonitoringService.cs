using System;
using System.ServiceModel;
using TRobot.Core.Services.Contracts.Data;
using TRobot.Communication.Events;

namespace TRobot.Communication.Services.Monitoring
{    
    [ServiceContract(
        SessionMode = SessionMode.Required,
        ConfigurationName = "Monitoring Service",        
        CallbackContract = typeof(IRobotTrajectoryMonitoringServiceCallback),
        Name = "Monitoring Service",        
        Namespace = "http://trobot/monitoring"
    )]
    public interface IRobotTrajectoryMonitoringService : IService
    {
        [OperationContract]  
        void SetupRobotTrajectory(RobotDescartesTrajectory robotTrajectory);

        [OperationContract]
        void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotTrajectory);

        [OperationContract]
        void TestOperation(Robot robot);

        event EventHandler<TrajectorySetEventArguments> RobotTrajectorySet;
        event EventHandler<RobotPositionUpdatedEventArguments> RobotPositionUpdated;
        event EventHandler<RobotTestEventArguments> TestEvent;
    }
}
