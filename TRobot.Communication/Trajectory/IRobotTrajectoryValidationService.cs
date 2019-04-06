using System.ServiceModel;

namespace TRobot.Communication.Trajectory
{    
    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IRobotTrajectoryValidationServiceCallback),
        Name = "RobotTrajectoryValidationService"
    )]
    public interface IRobotTrajectoryValidationService
    {
        [OperationContract]  
        void ValidateRobotTrajectory(RobotDescartesTrajectoryPoints robotTrajectory);
    }
}
