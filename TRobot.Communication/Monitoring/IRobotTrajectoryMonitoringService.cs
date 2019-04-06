using System.ServiceModel;
using TRobot.Communication.Contracts.Data;
using TRobot.Communication.Monitoring;

namespace TRobot.Communication.Trajectory
{    
    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IRobotTrajectoryMonitoringServiceCallback),
        Name = "IRobotTrajectoryMonitoringService",
        Namespace = "http://trobot/monitoring"
    )]
    public interface IRobotTrajectoryMonitoringService
    {
        [OperationContract]  
        void SetupRobotTrajectory(RobotDescartesTrajectory robotTrajectory);

        [OperationContract]
        void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotTrajectory);
    }
}
