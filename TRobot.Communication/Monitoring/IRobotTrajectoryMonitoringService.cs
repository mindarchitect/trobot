using System.ServiceModel;

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
    }
}
