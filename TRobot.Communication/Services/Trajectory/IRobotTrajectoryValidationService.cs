using System.ServiceModel;
using TRobot.Core;
using TRobot.Core.Services.Contracts.Data;

namespace TRobot.Communication.Services.Trajectory
{    
    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(IRobotTrajectoryValidationServiceCallback),
        Name = "RobotTrajectoryValidationService",
        Namespace = "http://trobot/validation"
    )]
    public interface IRobotTrajectoryValidationService : IService
    {
        DescartesRobotFactory DescartesRobotFactory
        {
            get;
            set;
        }

        [OperationContract]  
        void ValidateRobotTrajectory(RobotDescartesTrajectory robotTrajectory);
    }
}
