using System.ServiceModel;
using TRobot.Communication.Contracts.Data;

namespace TRobot.Communication.Trajectory
{    
    [ServiceContract(
        SessionMode = SessionMode.Required,        
        Name = "IRobotTrajectoryMonitoringService"
    )]
    public interface IRobotTrajectoryMonitoringService
    {
        [OperationContract]  
        void PlotRobotTrajectory(RobotDescartesTrajectory robotTrajectory);

        [OperationContract]
        void UpdateRobotPosition(RobotDescartesTrajectoryPosition robotTrajectory);
    }
}
