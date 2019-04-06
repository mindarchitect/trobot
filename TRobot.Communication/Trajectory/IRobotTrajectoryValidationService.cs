using System.ServiceModel;
using TRobot.Communication.Contracts.Data;

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
        void ValidateRobotTrajectory(RobotDescartesTrajectory robotTrajectory);
    }
}
